using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Models.Games;
using FairwayAPI.Models;
using FairwayAPI.Models.Leagues;
using FairwayAPI.Models.Inputs;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : Controller
    {
        private readonly ClubService _clubService;
        private readonly LeagueService _leagueService;
        private readonly LeagueGameReceiptService _leagueGameReceiptService;
        private readonly GameService _gameService;
        private readonly UserService _userService;
        private readonly CourseService _courseService;
        public LeagueController(ClubService clubService, LeagueService leagueService, LeagueGameReceiptService leagueGameReceiptService, GameService gameService, UserService userService, CourseService courseService) 
        {
            _clubService = clubService;
            _leagueService = leagueService;
            _leagueGameReceiptService = leagueGameReceiptService;
            _gameService = gameService;
            _userService = userService;
            _courseService = courseService;
        }

        [HttpPost("GetAllClubLeagues")]
        public ActionResult GetAllClubLeagues([FromBody] ClubIdInput input)
        {
            List<League> leagues = _leagueService.GetAllClubLeagues(input.clubId);
            if (leagues == null)
            {
                return NotFound("No Leagues Found for club");
            }
            return Ok(leagues);
            
        }

        // Get all league games
        [HttpPost("GetAllLeagueGames")]
        public ActionResult GetAllLeagueGames([FromBody] LeagueIdInput input)
        {
            List<LeagueGameReceipt> receipts = _leagueGameReceiptService.GetLeagueGameReceipts(input.leagueId);
            if (receipts != null)
            {
                List<string> gameIds = receipts.Select(r => r.GameId).ToList();
                List<Game> games = _gameService.GetGames(gameIds);

                return Ok(games);
            }
            return NotFound("No league games found");
        }

        // Generate League Table
        // Generate league Table with parameters (Dates, Game threshold...)

        [HttpPost("GenerateLeagueTable")]
        public ActionResult GenerateLeagueTable([FromBody] GenerateLeagueTableInput input)
        {
            League league = _leagueService.GetLeague(input.leagueId);
            if (league != null)
            {
                Club club = _clubService.GetClub(league.Club);
                if (club.Members != null)
                {
                    
                    List<User> players = _userService.GetUsers(club.Members.ToList());
                    List<LeagueTableRecord> leagueTableRecords = new List<LeagueTableRecord>();

                    List<LeagueGameReceipt> receipts = _leagueGameReceiptService.GetLeagueGameReceipts(input.leagueId);
                    List<Game> leagueGames = _gameService.GetGames(
                        receipts.Select(r => r.GameId).ToList()
                        );

                    if (!input.startDate.Equals("") && !input.endDate.Equals(""))
                    {
                        leagueGames = leagueGames.Where(g => g.Date >= DateTime.Parse(input.startDate) && g.Date <= DateTime.Parse(input.endDate)).ToList();
                    }

                    // Get all games user was part of in league, count num games, get points from top 16 games using game result
                    foreach (User user in players)
                    {
                        
                        List<Game> playerGames = leagueGames.Where(g => g.Players.Contains(user.Id)).ToList();
                        List<GameResultRecord> playerResults = new List<GameResultRecord>();
                        //List<GameResultRecord> playerResults = playerGames.Select(g => g.Result.Results[user.Id]).ToList(); // Possibly works the same as the loop
                        int totalPoints = 0;
                        int numGames = playerResults.Count;
                        double playerHandicap = 0;

                        if (numGames > 0)
                        {
                            foreach (Game game in playerGames)
                            {
                                playerResults.Add(game.Result.Results[user.Id]);
                            }

                            playerResults = playerResults.OrderBy(r => r.Points).ToList();
                            if (playerResults.Count > input.gameThreshold)
                            {
                                playerResults = playerResults.Take(input.gameThreshold).ToList();
                            }
                            totalPoints = playerResults.Sum(r => r.Points);
                            playerHandicap = _gameService.GetUserHandicapIndex(user.Id, playerGames, _courseService);
                        }
                        LeagueTableRecord record = new LeagueTableRecord(user.Name, numGames, totalPoints, playerHandicap);
                        leagueTableRecords.Add(record);
                    }

                    leagueTableRecords = leagueTableRecords.OrderByDescending(r => r.Points).ToList();
                    LeagueTable leagueTable = new LeagueTable(league.Name, league.StartDate, leagueTableRecords);
                    return Ok(leagueTable);

                }
                return BadRequest("Unable to create league table");
            }
            return NotFound("No League Found");
        }

        // Start League
        [HttpPost("StartLeague")]
        public ActionResult StartLeague([FromBody] StartLeagueInput input)
        {
            League league = new League(input.clubId, input.name, DateTime.Parse(input.startDate));
            _leagueService.CreateLeague(league);

            return Ok("League successfully created");
        }

        // Close league
        // Not sure if this is necessary or if it should just automatically close when end date arrives
        [HttpPost("CloseLeague")]
        public ActionResult EndLeague([FromBody] LeagueIdInput input)
        {
            League league = _leagueService.GetLeague(input.leagueId);
            if (league == null)
            {
                return NotFound("Can't find league with that ID");
            }
            league.Active = false;
            _leagueService.UpdateLeague(input.leagueId, league);
            return Ok("League successfullly closed");
        }
       
    }

    public class GenerateLeagueTableInput
    {
        public string leagueId { get; set; }
        public string startDate { get; set; } = "";
        public string endDate { get; set; } = "";
        public int gameThreshold { get; set; } = 16;
    }

    public class StartLeagueInput
    {
        public string clubId { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
    }
}
