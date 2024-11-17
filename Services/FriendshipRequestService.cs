using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class FriendshipRequestService
    {
        private readonly IMongoCollection<FriendshipRequest> _friendshipRequests;

        public FriendshipRequestService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _friendshipRequests = database.GetCollection<FriendshipRequest>("FriendshipRequest");
        }

        public void CreateFriendshipRequest(FriendshipRequest request) => _friendshipRequests.InsertOne(request);

        public FriendshipRequest GetFriendshipRequest(string id) => _friendshipRequests.Find(request => request.Id == id).FirstOrDefault();

        public List<FriendshipRequest> GetFriendshipRequests(List<string> ids) => _friendshipRequests.Find(request => ids.Contains(request.Id)).ToList();

        public List<FriendshipRequest> GetAllFriendshipRequests() => _friendshipRequests.Find(r => true).ToList();

        public void UpdateFriendshipRequest(string id, FriendshipRequest request) => _friendshipRequests.ReplaceOne(request => request.Id == id, request);

        public void DeleteFriendshipRequest(string id) => _friendshipRequests.DeleteOne(r => r.Id == id);
    }
}
