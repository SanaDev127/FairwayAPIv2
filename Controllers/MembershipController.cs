using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Models.Inputs;
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
        public ActionResult GenerateClubInvite([FromBody] GenerateClubInviteInput input)
        {
            ClubInvite invite = new ClubInvite(input.clubId, input.userId);
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
        public ActionResult AcceptClubInvite([FromBody] AcceptClubInviteInput input)
        {
            // Stop user from accepting if they're already in
            ClubInvite invite = _clubInviteService.GetClubInvite(input.inviteId);

            Club club = _clubService.GetClub(invite.Club);
            User player = _userService.GetUser(input.userId);
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
                player_handicap = _gameService.GetUserHandicapIndex(input.userId, player_games, _courseService);
            }

            MembershipRequest request = new MembershipRequest(invite.Club, input.userId, player_handicap);

            _membershipRequestService.CreateMembershipRequest(request);

            _clubInviteService.DeleteClubInvite(input.inviteId);

            return Ok("Membership request sent successfully");
        }

        [HttpPost("GetAllClubMembershipRequests")]
        public ActionResult GetAllClubMembershipRequests([FromBody] ClubIdInput input)
        {
            List<MembershipRequest> requests = _membershipRequestService.GetAllMembershipRequests().Where(r => r.Club == input.clubId).ToList();
            return Ok(requests);
        }

        //Reject club invite (Delete it)

        // Reject Membership Request (Delete it)

        // AcceptMembershipRequest
        // Add user to club by accepting membership request
        [HttpPost("AcceptMembershipRequest")]
        public ActionResult AcceptMembershipRequest([FromBody] RequestIdInput input)
        {
            MembershipRequest request = _membershipRequestService.GetMemebershipRequest(input.requestId);
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

            _membershipRequestService.DeleteMembershipRequest(input.requestId);
            return Ok("Player has successfully been added to club");
        }
    }

    public class GenerateClubInviteInput
    {
        public string clubId { get; set; }
        public string userId { get; set; }
    }

    public class AcceptClubInviteInput
    {
        public string userId { get; set; }
        public string inviteId { get; set; }
    }
    
}
