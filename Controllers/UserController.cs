using FairwayAPI.Models.Clubs;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;
using FairwayAPI.Models;
using FairwayAPI.Models.Games;
using System.Numerics;
using FairwayAPI.Models.Courses;
using System.Reflection;
using System.Diagnostics;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json;
using FairwayAPI.Models.Inputs;

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
        public ActionResult CreateUser([FromBody] CreateUserInput input)
        {
            User newUser = new User(input.username, input.userId, input.email);
            _userService.CreateUser(newUser);
            return Ok();
        }

        [HttpPost("GetUser")]
        public ActionResult GetUser([FromBody]UserIdInput input)
        {
            Trace.WriteLine("Trying to see ID");
            Trace.WriteLine(input.userId);
            User player = _userService.GetUser(input.userId);
            if (player == null)
            {
                return NotFound("No player found with that ID");
            }
            return Ok(player);
        }

        [HttpPost("GetAllGameInvites")]
        public ActionResult GetAllGameInvites([FromBody] UserIdInput input)
        {
            var invites = _gameInviteService.GetUserGameInvites(input.userId);
            return Ok(invites);
        }

        [HttpPost("CreateBuddyInvite")]
        public ActionResult CreateBuddyInvite([FromBody] SenderIdInput input)
        {
            BuddyInvite invite = new BuddyInvite(input.senderId);
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
        public ActionResult AcceptBuddyInvite([FromBody] AcceptBuddyInviteInput input)
        {
            BuddyInvite invite = _buddyInviteService.GetBuddyInvite(input.InviteId);

            User sender = _userService.GetUser(invite.Sender);
            User recipient = _userService.GetUser(input.UserId);

            sender.Friends ??= [];
            if (sender.Friends.Contains(input.UserId))
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
        public ActionResult AcceptFriendshipRequest([FromBody] RequestIdInput input)
        {
            FriendshipRequest request = _friendshipRequestService.GetFriendshipRequest(input.requestId);
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

            _friendshipRequestService.DeleteFriendshipRequest(input.requestId);

            return Ok("Players have been added to each other's networks");
        }

        [HttpPost("GetAllBuddies")]
        public ActionResult GetAllBuddies([FromBody] UserIdInput input)
        {
            try
            {
                User user = _userService.GetUser(input.userId);
                List<User> buddies = _userService.GetUsers(user.Friends.ToList());
                return Ok(buddies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetUserByFirebaseId")]
        public ActionResult GetUserByFirebaseId([FromBody] FirebaseIdInput input)
        {
            User user = _userService.GetAllUsers().FirstOrDefault(u => u.UserID == input.firebaseId);
            if (user == null)
            {
                return NotFound("No user found with that firebase ID");
            }
            return Ok(user);
        }

        [HttpPost("GetAllUserFriendshipRequests")]
        public ActionResult GetAllUserFriendshipRequests([FromBody] UserIdInput input)
        {
            List<FriendshipRequest> requests = _friendshipRequestService.GetAllFriendshipRequests().Where(r => r.RecipientId == input.userId).ToList();
            return Ok(requests);
        }

        [HttpPost("GetAllUserClubs")]
        public ActionResult GetAllUserClubs([FromBody] UserIdInput input)
        {
            User player = _userService.GetUser(input.userId);
            List<Club> userClubs = _clubService.GetClubs(player.Clubs.ToList());
            return Ok(userClubs);
        }

        //Reject buddy invite (Delete it)

        // Reject friendship Request (Delete it)

        // EditUserDetails
        // Add more personal details

    }

    public class AcceptBuddyInviteInput
    {
        public string UserId { get; set; }
        public string InviteId { get; set; }
    }

    public class FirebaseIdInput
    {
        public string firebaseId { get; set; }
    }
    

    


    
}
