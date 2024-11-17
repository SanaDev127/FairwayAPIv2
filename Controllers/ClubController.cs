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
        private readonly ClubGameReceiptService _clubGameReceiptService;

        public ClubController(ClubService clubService, GameService gameService, UpcomingGameService upcomingGameService, UserService userService, MembershipRequestService membershipRequestService, ClubGameReceiptService clubGameReceiptService)
        {
            _clubService = clubService;
            _gameService = gameService;
            _upcomingGameService = upcomingGameService;
            _userService = userService;
            _membershipRequestService = membershipRequestService;
            _clubGameReceiptService = clubGameReceiptService;
        }

        // Create Club
        [HttpPost("CreateClub")]
        //Might just get name and use that to create club
        public ActionResult CreateClub(string clubName, string creatorId)
        {
            User creator = _userService.GetUser(creatorId);
            Club club = new Club(clubName, creatorId);
            club.Members ??= [];
            club.Members = club.Members.Append(creatorId).ToArray();
            string clubId = _clubService.CreateClub(club);

            creator.Clubs = [.. creator.Clubs, clubId];
            _userService.UpdateUser(creator.Id, creator);

            return Ok("Club successfully created");
        }
        
        // Get Club's recently played games. 
        [HttpPost("GetClubsRecentGames")]
        public ActionResult GetClubsRecentGames(string clubId)
        {
            Club club = _clubService.GetClub(clubId);
            var recentGameIds = _clubGameReceiptService.GetClubGameReceiptsByClubId(clubId)
                .OrderByDescending(receipt => receipt.Date)
                .TakeLast(3)
                .Select(receipt => receipt.GameId);
           
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
        [HttpPost("GetAllClubGames")]
        public ActionResult<List<Game>> GetAllClubsGames(string id, string? sDate = null, string? eDate = null)
        {

            Club club = _clubService.GetClub(id);
            var gameIds = _clubGameReceiptService.GetAllClubGameReceipts()
                .Where(r => r.ClubId == id)
                .Select(r => r.GameId);

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
        [HttpPost("GetClubUpcomingGames")]
        public ActionResult<List<UpcomingGame>> GetClubUpcomingGames(string clubId)
        {
            Club club = _clubService.GetClub(clubId);
            //var gameIds = club?.UpcomingGames;
            var games = _upcomingGameService.GetAllUpcomingGames()
               .Where(g => g.Club.Equals(clubId) && g.HasSpace == true)
               ;
            if (games == null)
            {
                return NoContent();
            }
           // var games = _upcomingGameService.GetUpcomingGames(gameIds.ToList())
            //    .Where(g => g.HasSpace == true);

            return Ok(games);

        }

        // Get all upcoming games? All upcoming games where user has rsvp'd //RSVPing adds the game to the user's upcoming game list
        [HttpPost("GetUsersUpcomingGames")]
        public ActionResult<List<UpcomingGame>> GetUsersUpcomingGames(string userId)
        {
            User user = _userService.GetUser(userId);
            var games = _upcomingGameService.GetAllUpcomingGames()
                .Where(g => g.Players.Contains(user.Id));
            if (games == null)
            {
                return NoContent();
            }
            return Ok(games);
        }

        // Create Upcoming Game
        [HttpPost("CreateUpcomingGame")]
        public ActionResult CreateUpcomingGame(UpcomingGame upcomingGame)
        {
            //Make sure the organiser's ID ends up in here too
            _upcomingGameService.CreateUpcomingGame(upcomingGame);
            return Ok("Upcoming Game Successfully created");
        }

        // RSVP to Upcoming game (Edit game details when new person rsvp's. add player and reduce number of slots)
        // Pretty sure this works
        [HttpPut("RsvpToGame")]
        public ActionResult RsvpToGame(string userId, string gameId)
        {
            UpcomingGame game = _upcomingGameService.GetUpcomingGame(gameId);

            if (game.AvailableSlots > 0)
            {
                if (game.Players.Contains(userId))
                {
                    return BadRequest("You have already rsvp'd to this game");
                }
                game.AvailableSlots--;
                game.Players = game?.Players?.Append(userId).ToArray();
                _upcomingGameService.UpdateUpcomingGame(gameId, game);
                if (game.AvailableSlots == 0)
                {
                    game.HasSpace = false;
                }
            }
            return Ok("You have successfully RSVP'd");
        }

        [HttpPost("GetAllClubMembers")]
        public ActionResult GetAllClubMembers(string clubId)
        {
            Club club = _clubService.GetClub(clubId);
            List<User> members = _userService.GetUsers([.. club.Members]);
            return Ok(members);
        }

        

      

        // Add new admin
        // Get new admin details, add them to admin firebase table. Remove old admin's details. Change admin field on club
    }
}
