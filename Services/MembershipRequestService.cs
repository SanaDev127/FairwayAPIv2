using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class MembershipRequestService
    {
        private readonly IMongoCollection<MembershipRequest> _membershipRequests;

        public MembershipRequestService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _membershipRequests = database.GetCollection<MembershipRequest>("MembershipRequest");
        }

        public void CreateMembershipRequest(MembershipRequest request) => _membershipRequests.InsertOne(request);

        public MembershipRequest GetMemebershipRequest(string id) => _membershipRequests.Find(request => request.Id == id).FirstOrDefault();

        public List<MembershipRequest> GetMembershipRequests(List<string> ids) => _membershipRequests.Find(request => ids.Contains(request.Id)).ToList();

        public List<MembershipRequest> GetAllMembershipRequests() => _membershipRequests.Find(r => true).ToList();

        public void UpdateMembershipRequest(string id, MembershipRequest request) => _membershipRequests.ReplaceOne(request => request.Id == id, request);

        public void DeleteMembershipRequest(string id) => _membershipRequests.DeleteOne(request => request.Id == id);
    }
}
