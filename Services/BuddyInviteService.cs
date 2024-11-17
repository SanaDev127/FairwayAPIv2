using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class BuddyInviteService
    {
        private readonly IMongoCollection<BuddyInvite> _buddyInvites;

        public BuddyInviteService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _buddyInvites = database.GetCollection<BuddyInvite>("BuddyInvite");
        }

        public void CreateBuddyInvite(BuddyInvite invite) => _buddyInvites.InsertOne(invite);

        //Necessary? Maybe for admin, later on
        public BuddyInvite GetBuddyInvite(string id) => _buddyInvites.Find(invite => invite.Id == id).FirstOrDefault();

        public List<BuddyInvite> GetBuddyInvites(List<string> ids) => _buddyInvites.Find(invite => ids.Contains(invite.Id)).ToList();

        public List<BuddyInvite> GetAllBuddyInvites() => _buddyInvites.Find(invites => true).ToList();

        public void UpdateBuddyInvite(string id, BuddyInvite invite) => _buddyInvites.ReplaceOne(invite => invite.Id == id, invite);

        public void DeleteBuddyInvite(string id) => _buddyInvites.DeleteOne(r => r.Id == id);
    }
}
