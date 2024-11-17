using FairwayAPI.Models.Clubs;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models;
using FairwayAPI.Models.Games;
using System.Numerics;
using FairwayAPI.Models.Courses;
using System.Reflection;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
      
        private readonly OngoingGameService _ongoingGameService;
        private readonly UserService _userService;
        private readonly GameInviteService _gameInviteService;
        private readonly BuddyInviteService _buddyInviteService;
        private readonly FriendshipRequestService _friendshipRequestService;
        private readonly ClubService _clubService;
        public UserController( OngoingGameService ongoingGameService, UserService userService, GameInviteService gameInviteService, BuddyInviteService buddyInviteService, FriendshipRequestService friendshipRequestService, ClubService clubService)
        {
            _ongoingGameService = ongoingGameService;
            _userService = userService;
            _gameInviteService = gameInviteService;
            _buddyInviteService = buddyInviteService;
            _friendshipRequestService = friendshipRequestService;
            _clubService = clubService;
        }

        // Create User (They're gonna be created with firebase and stored in that db, but we need to store additional details here)
        [HttpPost("CreateUser")]
        public ActionResult CreateUser(string username, string userId, string email)
        {
            User newUser = new User(username, userId, email);
            _userService.CreateUser(newUser);
            return Ok();
        }

        [HttpPost("GetUser")]
        public ActionResult GetUser(string userId)
        {
            User player = _userService.GetUser(userId);
            if (player == null)
            {
                return NotFound("No player found with that ID");
            }
            return Ok(player);
        }

        [HttpGet("GetAllGameInvites")]
        public ActionResult GetAllGameInvites(string playerId)
        {
            var invites = _gameInviteService.GetUserGameInvites(playerId);
            return Ok(invites);
        }

        [HttpPost("CreateBuddyInvite")]
        public ActionResult CreateBuddyInvite(string senderId)
        {
            BuddyInvite invite = new BuddyInvite(senderId);
            _buddyInviteService.CreateBuddyInvite(invite);
            if (invite.Id != null)
            {
                return Ok("Invite sent successfully");
            }
            else
            {
                return BadRequest("Unable to send invite");
            }
           
        }

        [HttpPost("AcceptBuddyInvite")]
        public ActionResult AcceptBuddyInvite(string userId, string inviteId)
        {
            BuddyInvite invite = _buddyInviteService.GetBuddyInvite(inviteId);

            User sender = _userService.GetUser(invite.Sender);
            User recipient = _userService.GetUser(userId);

            sender.Friends ??= [];
            if (sender.Friends.Contains(userId))
            {
                return BadRequest("This person is already in your network");
            }
            recipient.Friends ??= [];
            if (recipient.Friends.Contains(sender.Id))
            {
                return BadRequest("You are already in this person's network");
            }

            _buddyInviteService.DeleteBuddyInvite(invite.Id);

            FriendshipRequest request = new FriendshipRequest(recipient.Id, sender.Id);
            _friendshipRequestService.CreateFriendshipRequest(request);

            return Ok("Friendship request has been sent");
        }

        [HttpPost("AcceptFriendshipRequest")]
        public ActionResult AcceptFriendshipRequest(string requestId)
        {
            FriendshipRequest request = _friendshipRequestService.GetFriendshipRequest(requestId);
            User requester = _userService.GetUser(request.RequesterId);
            User recipient = _userService.GetUser(request.RecipientId);

            requester.Friends ??= [];
            var requester_buddies = requester.Friends.Append(recipient.Id);
            requester.Friends = requester_buddies.ToArray();
            _userService.UpdateUser(requester.Id, requester);

            recipient.Friends ??= [];
            var recipient_buddies = recipient.Friends.Append(requester.Id);
            recipient.Friends = recipient_buddies.ToArray();
            _userService.UpdateUser(recipient.Id, recipient);

            _friendshipRequestService.DeleteFriendshipRequest(requestId);

            return Ok("Players have been added to each other's networks");
        }

        [HttpGet("GetAllBuddies")]
        public ActionResult GetAllBuddies(string userId)
        {
            User user = _userService.GetUser(userId);
            List<User> buddies= _userService.GetUsers(user.Friends.ToList());
            return Ok(buddies);
        }

        [HttpGet("GetUserByFirebaseId")]
        public ActionResult GetUserByFirebaseId(string firebaseId)
        {
            User user = _userService.GetAllUsers().FirstOrDefault(u => u.UserID == firebaseId);
            if (user == null)
            {
                return NotFound("No user found with that firebase ID");
            }
            return Ok(user);
        }

        [HttpGet("GetAllUserFriendshipRequests")]
        public ActionResult GetAllUserFriendshipRequests(string userId)
        {
            List<FriendshipRequest> requests = _friendshipRequestService.GetAllFriendshipRequests().Where(r => r.RecipientId == userId).ToList();
            return Ok(requests);
        }

        [HttpGet("GetAllUserClubs")]
        public ActionResult GetAllUserClubs(string userId)
        {
            User player = _userService.GetUser(userId);
            List<Club> userClubs = _clubService.GetClubs(player.Clubs.ToList());
            return Ok(userClubs);
        }

        //Reject buddy invite (Delete it)

        // Reject friendship Request (Delete it)

        // EditUserDetails
        // Add more personal details

    }
}
