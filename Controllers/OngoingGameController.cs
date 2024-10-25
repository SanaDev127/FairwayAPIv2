using FairwayAPI.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Games;
using FairwayAPI.Models;

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
        private readonly UpcomingGameService _upcomingGameService;

        public OngoingGameController(CourseService courseService, OngoingGameService ongoingGameService, UserService userService, GameInviteService gameInviteService, UpcomingGameService upcomingGameService)
        {
            _courseService = courseService;
            _ongoingGameService = ongoingGameService;
            _userService = userService;
            _gameInviteService = gameInviteService;
            _upcomingGameService = upcomingGameService;

        }

        //InvitePlayer? For now it just adds the invite to the db. In future it should send a push notification
        //Not finished, might need to take data as params and then create invite instead of taking invite as param. Need to change a lot of them to be like this
        [HttpPost("InvitePlayer")]
        public ActionResult InvitePlayer(GameInvite invite)
        {
            _gameInviteService.CreateGameInvite(invite);
            return Ok();
        }

        //DisplayCourses or GetGourses or Search courses too
        // For now, returns a list of available course names. They're attached to their ids (key value pair). When user selects it, it the id is used to create the game
        [HttpGet("GetCourseNames")]
        public ActionResult GetCourseNames()
        {
            List<Course> courses = _courseService.GetAllCourses();
            Dictionary<string, string> courseDetails = new Dictionary<string, string>();
            foreach (var course in courses)
            {
                courseDetails[course.CourseName] = course.Id;
            }
            return Ok(courseDetails);
        }
        //StartGame
        [HttpPost("StartGame")]
        public ActionResult CreateGame(OngoingGame game)
        {
            _ongoingGameService.CreateOngoingGame(game);
            // Start websocket connection or whatever
            return Ok();
        }

        // Join game after receiving an invite. Also need method to send invite
        [HttpPost("Join Game")]
        public ActionResult JoinGame(string gameId, string userId)
        {
            // All this can be put into a method in the ongoing games service...but not very necessary
            OngoingGame game = _ongoingGameService.GetOngoingGame(gameId);
            User player = _userService.GetUser(userId);

            var players_list = game.Players.Append(player).ToArray();
            game.Players = players_list;
            _ongoingGameService.UpdateOngoingGame(game.Id, game);

            var games_list = player.ActiveGames.Append(gameId);
            player.ActiveGames = games_list.ToArray();
            _userService.UpdateUser(player.Id, player);

            return Ok();
        }

        // Get User's ongoing games
        [HttpGet("GetUserOngoingGames")]
        public ActionResult<List<OngoingGame>> GetUserOngoingGames(string id)
        {
            User user = _userService.GetUser(id);
            var activeGameIds = user.ActiveGames;
            if (activeGameIds == null)
            {
                return NoContent();
            }
            var activeGames = _ongoingGameService.GetOngoingGames(activeGameIds.ToList());
            return Ok(activeGames);
        }

        //GetGame?
        [HttpGet("GetOnGoingGame")]
        public ActionResult GetOngoingGame(string id)
        {
            var game = _ongoingGameService.GetOngoingGame(id);
            return Ok(game);
        }


        //Generate Scorecard for active game
        [HttpGet("GetOngoingGameScorecard")]
        public ActionResult GetOngoingGameScorecard(string gameId)
        {
            OngoingGame game = _ongoingGameService.GetOngoingGame(gameId);

            DetailScorecard scorecard = new DetailScorecard(game.Course.Scorecard.Length, game.Players.Length);
            for (int i = 0; i < game.Course.Scorecard.Length; i++)
            {
                scorecard.Holes[i] = game.Course.Scorecard[i].Number;
                scorecard.Distances[i] = game.Course.Scorecard[i].Tees.teeBox1.yards;
                scorecard.Pars[i] = game.Course.Scorecard[i].Par;
                scorecard.PlayerNames[i] = game.Players[i].Name;

                for (int j = 0; j < game.Players.Length; j++)
                {
                    scorecard.Strokes[j, i] = game.Scorecard.PlayerScorecards[j].Strokes[i];
                    scorecard.Points[j, i] = game.Scorecard.PlayerScorecards[j].Points[i];
                }

            }
            return Ok(scorecard);

        }

        //Update Ongoing Game
        [HttpPut("UpdateOngoingGame")]
        public ActionResult UpdateOngoingGame(OngoingGame game)
        {
            _ongoingGameService.UpdateOngoingGame(game.Id, game);
            return Ok();
        }

        //EndGame
        [HttpPost("SaveGame")]
        public ActionResult SaveGame(string gameId)
        {
            OngoingGame game = _ongoingGameService.GetOngoingGame(gameId);
            // Need to decide what attributes are gonna be in ongoing game and use them to create a proper game object
            // End websocket connection
            Game newGame = new Game();
            return Ok();

        }


    }
}
