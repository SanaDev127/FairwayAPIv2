using FairwayAPI.Models;
using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Games;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using System.Linq;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly UserService _userService;
        private readonly GameService _gameService;
        private readonly OngoingGameService _ongoingGameService;
        private readonly CourseService _courseService;

        public GameController(UserService userService, GameService gameService, OngoingGameService ongoingGameService, CourseService courseService)
        {
            _userService = userService;
            _ongoingGameService = ongoingGameService;
            _gameService = gameService;
            _courseService = courseService;
        }

        [HttpPost("ShowGameResults")]
        public ActionResult<GameResult> ShowGameResults(string gameId)
        {
            Game game = _gameService.GetGame(gameId);
            Course course = _courseService.GetCourse(game.Course);

            DetailScorecard sc = _gameService.GenerateDetailScorecard(game, course, _userService);
            Dictionary<string, int> scores = _gameService.GetGameResults(game);
            GameResult  gr = new GameResult(scores, sc);
           
            return Ok(gr);

        }

        // Get User's recently played games. 
        [HttpGet("GetUserRecentGames")]
        public ActionResult<List<Game>> GetUserRecentGames(string id)
        {
            User user = _userService.GetUser(id);
            var recentGameIds = user?.Games?.TakeLast(3);
            if (recentGameIds == null)
            {
                return NoContent();
            }
            var recentGames = _gameService.GetGames(recentGameIds.ToList());
            return Ok(recentGames);

        }

        // Get all user's games 
        // Want to be able to get games with certain participants
        // Want to be able to get games that were at a certain course
        [HttpGet("GetAllUsersGames")]
        public ActionResult<List<Game>> GetAllUsersGames(string id, string? sDate = null, string? eDate = null, string[]? participants = null)
        {

            User user = _userService.GetUser(id);
            var gameIds = user?.Games;
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
       
        [HttpGet("GetGameScorecard")]
        public ActionResult<DetailScorecard> GetGameScorecard(string gameId)
        {
            Game game = _gameService.GetGame(gameId);
            Course course = _courseService.GetCourse(game.Course);
            DetailScorecard scorecard = _gameService.GenerateDetailScorecard(game, course, _userService);
            return Ok(scorecard);

        }
        [HttpGet("GetUserHandicapIndex")]
        public ActionResult<double> GetUserHandicapIndex(string userId)
        {
            User user = _userService.GetUser(userId);
            string[] userGameIds = user.Games;
            List<Game> userGames = _gameService.GetGames(userGameIds.ToList());

            return _gameService.GetUserHandicapIndex(userId, userGames, _courseService);

        }


    }
}
