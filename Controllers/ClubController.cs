using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Services;
using FairwayAPI.Models;
using FairwayAPI.Models.Games;
using MongoDB.Driver.Linq;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : Controller
    {
        private readonly ClubService _clubService;
        private readonly GameService _gameService;
        private readonly UpcomingGameService _upcomingGameService;
        private readonly UserService _userService;
        private readonly MembershipRequestService _membershipRequestService;

        public ClubController(ClubService clubService, GameService gameService, UpcomingGameService upcomingGameService, UserService userService, MembershipRequestService membershipRequestService)
        {
            _clubService = clubService;
            _gameService = gameService;
            _upcomingGameService = upcomingGameService;
            _userService = userService;
            _membershipRequestService = membershipRequestService;

        }

        // Create Club
        [HttpPost("CreateClub")]
        public ActionResult CreateClub(Club club)
        {
            _clubService.CreateClub(club);
            return Ok();
        }
        // Start club game? starts a game but adds necessry club and leage details etc..
        // Add Club Game (Adds the game to the club's list of games and to the user's list of games) (This is how to keep track of club games)
        // Get Club's recently played games. 
        [HttpGet("GetClubsRecentGames")]
        public ActionResult GetClubsRecentGames(string clubId)
        {
            Club club = _clubService.GetClub(clubId);
            var recentGameIds = club?.Games?.TakeLast(3);
            if (recentGameIds == null)
            {
                return NoContent();
            }
            var recentGames = _gameService.GetGames(recentGameIds.ToList());
            return Ok(recentGames);
        }

        // Get all club's games (Maybe return a summary of the games for display purposes i.e. course, date, score... or this is done on client side
        // Get all club's games with query parameters i.e. date, course, participants?
        // Want to be able to get games that were at a certain course
        [HttpGet("GetAllUsersGames")]
        public ActionResult<List<Game>> GetAllClubsGames(string id, string? sDate = null, string? eDate = null, string[]? participants = null)
        {

            Club club = _clubService.GetClub(id);
            var gameIds = club?.Games;
            if (gameIds == null)
            {
                return NoContent();
            }
            var games = _gameService.GetGames(gameIds.ToList());

            if (sDate != null && eDate != null)
            {
                var startDate = DateTime.Parse(sDate);
                var endDate = DateTime.Parse(eDate);
                games = games.Where(g => g.Date >= startDate && g.Date <= endDate).ToList();
            }

            return Ok(games);

        }
        // Get all club's upcoming games? All the upcoming games for the club to create the notice board // Add an upcoming games list for club
        [HttpGet("GetClubUpcomingGames")]
        public ActionResult<List<UpcomingGame>> GetClubUpcomingGames(string clubId)
        {
            Club club = _clubService.GetClub(clubId);
            var gameIds = club?.UpcomingGames;
            if (gameIds == null)
            {
                return NoContent();
            }
            var games = _upcomingGameService.GetUpcomingGames(gameIds.ToList())
                .Where(g => g.HasSpace == true);

            return Ok(games);

        }

        // Get all upcoming games? All upcoming games where user has rsvp'd //RSVPing adds the game to the user's upcoming game list
        [HttpGet("GetAllUsersUpcomingGames")]
        public ActionResult<List<UpcomingGame>> GetUsersUpcomingGames(string userId)
        {
            User user = _userService.GetUser(userId);
            var games = _upcomingGameService.GetAllUpcomingGames()
                .Where(g => g.Players.Contains(userId));
            if (games == null)
            {
                return NoContent();
            }
            return Ok(games);
        }

        // Create Upcoming Game
        [HttpPost("CreateUpcomingGame")]
        public ActionResult CreateUpcomingGame(UpcomingGame game)
        {
            _upcomingGameService.CreateUpcomingGame(game);
            return Ok();
        }

        // RSVP to Upcoming game (Edit game details when new person rsvp's. add player and reduce number of slots)
        [HttpPut("RsvpToGame")]
        public ActionResult RsvpToGame(string userId, string gameId)
        {
            UpcomingGame game = _upcomingGameService.GetUpcomingGame(gameId);

            if (game.AvailableSlots > 0)
            {
                game.AvailableSlots--;
                game.Players = game?.Players?.Append(userId).ToArray();
                _upcomingGameService.UpdateUpcomingGame(gameId, game);
                if (game.AvailableSlots == 0)
                {
                    game.HasSpace = false;
                }
            }
            return Ok();
        }

        // Add user to club by accepting membership request
        [HttpPost("AcceptMembershipRequest")]
        public ActionResult AcceptMembershipRequest(string requestId)
        {
            MembershipRequest request = _membershipRequestService.GetMemebershipRequest(requestId);
            User player = _userService.GetUser(request.Player);
            Club club = _clubService.GetClub(request.Club);

            club.Members = club.Members.Append(request.Player).ToArray();
            player.Clubs = player.Clubs.Append(request.Player).ToArray();
            
            _membershipRequestService.DeleteMembershipRequest(requestId);
            return Ok();
        }
    }
}
