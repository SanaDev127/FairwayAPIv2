using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : Controller
    {
        private readonly ClubService _clubService;
        private readonly ClubInviteService _clubInviteService;
        private readonly MembershipRequestService _membershipRequestService;
        private readonly GameService _gameService;
        private readonly CourseService _courseService;
        private readonly UserService _userService;
        public MembershipController(CourseService courseService, GameService gameService, MembershipRequestService membershipRequestService, ClubService clubService, ClubInviteService clubInviteService, UserService userService)
        {
            _clubInviteService = clubInviteService;
            _clubService = clubService;
            _membershipRequestService = membershipRequestService;
            _gameService = gameService;
            _courseService = courseService;
            _userService = userService;
        }
       
        // Generate Invite
        [HttpPost("GenerateClubInvite")]
        public ActionResult GenerateClubInvite(string clubId, string userId)
        {
            ClubInvite invite = new ClubInvite(clubId, userId);
            _clubInviteService.CreateClubInvite(invite);
            if (invite.Id != null)
            {
                return Ok("Club invite sent successfully");
            }
            else
            {
                return BadRequest("Unable to send invite");
            }
           
        }
        
        // Accept Club invite?
        [HttpPost("AcceptClubInvite")]
        public ActionResult AcceptClubInvite(string userId, string inviteId)
        {
            // Stop user from accepting if they're already in
            ClubInvite invite = _clubInviteService.GetClubInvite(inviteId);

            Club club = _clubService.GetClub(invite.Club);
            User player = _userService.GetUser(userId);
            club.Members ??= [];
            if (club.Members.Contains(player.Id))
            {
                return BadRequest("User is already part of this club");
            }

            player.Games ??= [];
            var player_games = _gameService.GetGames(player.Games.ToList());
            double player_handicap = 54.0;
            if (player_games.Count > 0)
            {
                player_handicap = _gameService.GetUserHandicapIndex(userId, player_games, _courseService);
            }

            MembershipRequest request = new MembershipRequest(invite.Club, userId, player_handicap);

            _membershipRequestService.CreateMembershipRequest(request);

            _clubInviteService.DeleteClubInvite(inviteId);

            return Ok("Membership request sent successfully");
        }

        [HttpGet("GetAllClubMembershipRequests")]
        public ActionResult GetAllClubMembershipRequests(string clubId)
        {
            List<MembershipRequest> requests = _membershipRequestService.GetAllMembershipRequests().Where(r => r.Club == clubId).ToList();
            return Ok(requests);
        }

        //Reject club invite (Delete it)

        // Reject Membership Request (Delete it)

        // AcceptMembershipRequest
        // Add user to club by accepting membership request
        [HttpPost("AcceptMembershipRequest")]
        public ActionResult AcceptMembershipRequest(string requestId)
        {
            MembershipRequest request = _membershipRequestService.GetMemebershipRequest(requestId);
            if (request.Player == null || request.Club == null)
            {
                return BadRequest("Insufficient data in membership request");
            }
            User player = _userService.GetUser(request.Player);
            Club club = _clubService.GetClub(request.Club);

            club.Members ??= [];
            club.Members = club.Members.Append(request.Player).ToArray();
            _clubService.UpdateClub(club.Id, club);

            player.Clubs ??= [];
            player.Clubs = player.Clubs.Append(request.Player).ToArray();
            _userService.UpdateUser(player.Id, player);

            _membershipRequestService.DeleteMembershipRequest(requestId);
            return Ok("Player has successfully been added to club");
        }


    }
}
