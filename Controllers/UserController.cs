using FairwayAPI.Models.Clubs;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models;
using FairwayAPI.Models.Games;
using System.Numerics;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ClubService _clubService;
        private readonly GameService _gameService;
        private readonly OngoingGameService _ongoingGameService;
        private readonly UserService _userService;
        private readonly GameInviteService _gameInviteService;
        private readonly ClubInviteService _clubInviteService;
        private readonly MembershipRequestService _membershipRequestService;
        private readonly CourseService _courseService;

        public UserController(ClubService clubService, GameService gameService, OngoingGameService ongoingGameService, UserService userService, GameInviteService gameInviteService, ClubInviteService clubInviteService, MembershipRequestService membershipRequestService, CourseService courseService)
        {
            _clubService = clubService;
            _gameService = gameService;
            _ongoingGameService = ongoingGameService;
            _userService = userService;
            _gameInviteService = gameInviteService;
            _clubInviteService = clubInviteService;
            _membershipRequestService = membershipRequestService;
            _courseService = courseService;
        }

        // Create User (They're gonna be created with firebase and stored in that db, but we need to store additional details here)
        [HttpPost("CreateUser")]
        public ActionResult CreateUser(string userId, string username, string email)
        {
            User newUser = new User(userId, username, email);
            _userService.CreateUser(newUser);
            return Ok();
        }

        // Accept Game Invite?
        [HttpPost("AcceptGameInvite")]
        public ActionResult AcceptGameInvite(string userId, string inviteId)
        {
            GameInvite invite = _gameInviteService.GetGameInvite(inviteId);
            // All this can be put into a method in the ongoing games service...but not very necessary
            OngoingGame game = _ongoingGameService.GetOngoingGame(invite.GameID);
            User player = _userService.GetUser(userId);

            MembershipRequest request = new MembershipRequest();

            var players_list = game.Players.Append(player).ToArray();
            game.Players = players_list;
            _ongoingGameService.UpdateOngoingGame(game.Id, game);

            var games_list = player.ActiveGames.Append(game.Id);
            player.ActiveGames = games_list.ToArray();
            _userService.UpdateUser(player.Id, player);

            return Ok();
        }

        // Accept Club invite?
        [HttpPost("AcceptClubInvite")]
        public ActionResult AcceptClubInvite(string userId, string inviteId)
        {
            ClubInvite invite = _clubInviteService.GetClubInvite(inviteId);
            
            Club club = _clubService.GetClub(invite.Club);
            User player = _userService.GetUser(userId);
            var player_games = _gameService.GetGames(player.Games.ToList());
            double player_handicap = _gameService.GetUserHandicapIndex(userId, player_games, _courseService);

            MembershipRequest request = new MembershipRequest(invite.Club, userId, player_handicap);

            _membershipRequestService.CreateMembershipRequest(request);

            return Ok();
        }

        // Get All Invites (Any invites sent to play a game, become a buddy or join a club)
        // Get buddies (When setting up a game, you want to be able to send invites to your friends)


    }
}
