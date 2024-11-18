using FairwayAPI.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Games;
using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using System.Linq;
using FairwayAPI.Models.Inputs;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OngoingGameController : Controller
    {
        private readonly CourseService _courseService;
        private readonly OngoingGameService _ongoingGameService;
        private readonly UserService _userService;
        private readonly GameInviteService _gameInviteService;
        private readonly ClubGameReceiptService _clubGameReceiptService;
        private readonly GameService _gameService;
        private readonly LeagueGameReceiptService _leagueGameReceiptService;
        private readonly LeagueService _leagueService;

        public OngoingGameController(CourseService courseService, OngoingGameService ongoingGameService, UserService userService, GameInviteService gameInviteService, ClubGameReceiptService clubGameReceiptService, GameService gameService, LeagueGameReceiptService leagueGameReceiptService, LeagueService leagueService)
        {
            _courseService = courseService;
            _ongoingGameService = ongoingGameService;
            _userService = userService;
            _gameInviteService = gameInviteService;
            _clubGameReceiptService = clubGameReceiptService;
            _gameService = gameService;
            _leagueGameReceiptService = leagueGameReceiptService;
            _leagueService = leagueService;
        }

        //DisplayCourses or GetGourses or Search courses too
        // For now, returns a list of available course names. They're attached to their ids (key value pair). When user selects it, it the id is used to create the game
        [HttpPost("GetCourseNames")]
        public ActionResult GetCourseNames()
        {
            List<Course> courses = _courseService.GetAllCourses();
            if (courses != null)
            {
                Dictionary<string, string> courseDetails = new Dictionary<string, string>();
                foreach (var course in courses)
                {
                    courseDetails[course.CourseName] = course.Id;
                }
                return Ok(courseDetails);
            }
            else return NoContent();
        }
        //StartGame
        [HttpPost("StartGame")]
        // Pressing the start game button on the club site will pass the club id as a parameter, through that we'll know if it's a club game or not
        public ActionResult<string> StartGame([FromBody] StartGameInput input)
        {
            Course course = _courseService.GetCourse(input.courseId);
            OngoingGame game;
            User game_organiser = _userService.GetUser(input.organiserId);

            if (input.invitees == null || input.invitees.Length == 0)
            {
                game = new(game_organiser, course);
            }
            else
            {
                game = new OngoingGame(game_organiser, input.invitees, course);
            }

            double organiser_handicap = _gameService.GetUserHandicapIndex(input.organiserId, _gameService.GetUserGames(input.organiserId), _courseService);
            game.PlayerHandicaps ??= [];
            game.PlayerHandicaps = [.. game.PlayerHandicaps, organiser_handicap];
            game.Scorecard.Players = [.. game.Scorecard.Players, game_organiser];

            string gameId = _ongoingGameService.CreateOngoingGame(game);
            if (input.invitees != null && input.invitees.Length > 0)
            {
                foreach (string playerId in input.invitees)
                {
                    // In future figure out a way to send push notification
                    GameInvite invite = new GameInvite(gameId, playerId);
                    _gameInviteService.CreateGameInvite(invite);
                }
            }
            if (!input.clubId.Equals("") && input.clubId != null)
            {
                ClubGameReceipts receipt = new ClubGameReceipts(input.clubId, gameId, game.StartTime);
                _clubGameReceiptService.CreateClubGameReceipt(receipt);
            }
            game_organiser.ActiveGames ??= [];
            game_organiser.ActiveGames = [.. game_organiser.ActiveGames, gameId];
            _userService.UpdateUser(game_organiser.Id, game_organiser);

            return Ok(gameId);
        }

        // Join game after receiving an invite. Also need method to send invite
        [HttpPost("AcceptGameInvite")]
        public ActionResult AcceptGameInvite([FromBody] InviteIdInput input)
        {
            GameInvite invite = _gameInviteService.GetGameInvite(input.inviteId);
            OngoingGame game = _ongoingGameService.GetOngoingGame(invite.GameID);
            User player = _userService.GetUser(invite.RecipientID);

            if (game != null && player != null)
            {
                if (game.Players.Contains(player))
                {
                    return BadRequest("Player is already in the game");
                }
                game.Players = game.Players.Append(player).ToArray();

                double player_handicap = _gameService.GetUserHandicapIndex(player.Id, _gameService.GetUserGames(player.Id), _courseService);
                game.PlayerHandicaps = [.. game.PlayerHandicaps, player_handicap];

                game.Scorecard.Players = [.. game.Scorecard.Players, player];
                game.Scorecard.PlayerScorecards = [.. game.Scorecard.PlayerScorecards, new PlayerScorecard(player.Id, player.Name)];

                _ongoingGameService.UpdateOngoingGame(game.Id, game);

                player.ActiveGames ??= [];
                player.ActiveGames = player.ActiveGames.Append(game.Id).ToArray();
                _userService.UpdateUser(player.Id, player);

                _gameInviteService.DeleteGameInvite(invite.Id);

                return Ok(game);
            }
            else return NotFound("Ongoing Game Not Found");
        }

        // Get User's ongoing games
        // Maybe change how it works to look through active games and find one's user is a part of. This way you can remove active games field from user
        [HttpPost("GetUserOngoingGames")]
        public ActionResult<List<OngoingGame>> GetUserOngoingGames([FromBody] IdInput input)
        {
            User user = _userService.GetUser(input.id);
            var activeGameIds = user.ActiveGames;
            if (activeGameIds == null)
            {
                return NotFound("User has no ongoing games");
            }
            var activeGames = _ongoingGameService.GetOngoingGames(activeGameIds.ToList());
            return Ok(activeGames);
        }

        //GetGame?
        [HttpPost("GetOnGoingGame")]
        public ActionResult GetOngoingGame([FromBody] IdInput input)
        {
            var game = _ongoingGameService.GetOngoingGame(input.id);
            if (game == null)
            {
                return NotFound("No ongoing game found");
            }
            return Ok(game);
        }


        //Generate Scorecard for active game
        [HttpPost("GetOngoingGameScorecard")]
        public ActionResult GetOngoingGameScorecard([FromBody] GameIdInput input)
        {
            OngoingGame game = _ongoingGameService.GetOngoingGame(input.gameId);
            DetailScorecard scorecard = _ongoingGameService.GetOngoingGameScorecard(game);
            return Ok(scorecard);

        }

        //Update Ongoing Game
        [HttpPut("UpdateOngoingGame")]
        public ActionResult UpdateOngoingGame([FromBody] OngoingGame game)
        {
            _ongoingGameService.UpdateOngoingGame(game.Id, game);
            return Ok();
        }

        //EndGame
        [HttpPost("SaveGame")]
        public ActionResult SaveGame([FromBody] GameIdInput input)
        {
            OngoingGame game = _ongoingGameService.GetOngoingGame(input.gameId);
            
            // End websocket connection
            var player_ids = new List<string>();
            foreach (var player in game.Players)
            {
                User user = _userService.GetUser(player.Id);
                // May be unnecessary if I decide not to have the games field on a user
                user.Games ??= [];
                user.Games = user.Games.Append(input.gameId).ToArray();

                // Ugly. Must remove active games field
                user.ActiveGames ??= [];
                List<string> active_games = [.. user.ActiveGames];
                bool removed = active_games.Remove(input.gameId);
                if (removed)
                {
                    user.ActiveGames = [.. active_games];
                }
                _userService.UpdateUser(user.Id, user);

                player_ids.Add(player.Id);
            }

            List<GameResultRecord> results = new List<GameResultRecord>();
            foreach (var sc in game.Scorecard.PlayerScorecards)
            {
                string playerId = sc.PlayerId;
                string playerName = sc.PlayerName;
                int pointsTotal = sc.Points.Sum();
                int strokeTotal = sc.Strokes.Sum();
                int numHoles = game.Course.NumHoles;

                GameResultRecord record = new GameResultRecord(playerId, playerName, strokeTotal, pointsTotal, numHoles);   
                 
                results.Add(record);
            }
            
            DetailScorecard scorecard = _ongoingGameService.GetOngoingGameScorecard(game);

            GameResult gameResult = new GameResult(results, scorecard);

            Game newGame = new Game(
                game.Course.Id,
                game.StartTime,
                game.Organiser,
                game.Scorecard,
                player_ids.ToArray(),
                gameResult
                );

            string newGameId = _gameService.CreateGame(newGame);

            if (_clubGameReceiptService.GetClubGameReceiptByGameID(input.gameId) != null)
            {
                ClubGameReceipts gameReceipt = _clubGameReceiptService.GetClubGameReceiptByGameID(input.gameId);
                Console.WriteLine("Debugging");
                Console.WriteLine($"Old Game ID: {input.gameId}");
                Console.WriteLine($"New Game ID: {newGameId}");
                gameReceipt.GameId = newGameId;
                _clubGameReceiptService.UpdateClubGameReceipt(gameReceipt.Id, gameReceipt);
                Console.WriteLine($"Receipt Game ID: {gameReceipt.GameId}");

                if (game.Players.Length == 4)
                {
                    League league = _leagueService.GetLatestClubLeague(gameReceipt.ClubId);
                    LeagueGameReceipt leagueGameReceipt = new LeagueGameReceipt(league.Id, newGameId);
                    _leagueGameReceiptService.CreateLeagueGameReceipt(leagueGameReceipt);
                }
               
            }
            _ongoingGameService.DeleteOngoingGame(game.Id);
            return Ok();

        }

    }

    public class StartGameInput
    {
        public string organiserId { get; set; }
        public string courseId { get; set; }
        public string[]? invitees { get; set; }
        public string clubId { get; set; } = "";
    }
}
