﻿using FairwayAPI.Models;
using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Games;
using FairwayAPI.Models.Inputs;
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
       
        // Get User's recently played games. 
        [HttpPost("GetUserRecentGames")]
        public ActionResult<List<Game>> GetUserRecentGames(string id)
        {
            User user = _userService.GetUser(id);
            var recentGames = _gameService.GetAllGames()
                .Where(g => g.Players.Contains(id))
                .TakeLast(3)
                .ToList();
           
            return Ok(recentGames);

        }

        // Get all user's games 
        // Want to be able to get games with certain participants
        // Want to be able to get games that were at a certain course
        [HttpPost("GetAllUsersGames")]
        public ActionResult<List<Game>> GetAllUsersGames([FromBody] GetAllUserGamesInput input)
        {

            User user = _userService.GetUser(input.id);
            var gameIds = user?.Games;
            if (gameIds == null)
            {
                return NoContent();
            }
            var games = _gameService.GetGames(gameIds.ToList());

            if (input.sDate != null && input.eDate != null)
            {
                var startDate = DateTime.Parse(input.sDate);
                var endDate = DateTime.Parse(input.eDate);
                games = games.Where(g => g.Date >= startDate && g.Date <= endDate).ToList();
            }

            return Ok(games);

        }
      
        [HttpPost("GetUserHandicapIndex")]
        public ActionResult<double> GetUserHandicapIndex([FromBody] UserIdInput input)
        {
            User user = _userService.GetUser(input.userId);
            string[] userGameIds = user.Games;
            List<Game> userGames = _gameService.GetGames(userGameIds.ToList());
            double handicapIndex = 54.0;
            if (userGameIds.Length > 0)
            {
                handicapIndex = _gameService.GetUserHandicapIndex(input.userId, userGames, _courseService);
            }
            return handicapIndex;

        }

    }

    public class GetAllUserGamesInput
    {
        public string id {get; set;}
        public string? sDate { get; set; } = null;
        public string? eDate { get; set; } = null;
        public string[]? participants { get; set; } = null;

    }
}
