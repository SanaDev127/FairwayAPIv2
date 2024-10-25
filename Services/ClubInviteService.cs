using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class ClubInviteService
    {
        private readonly IMongoCollection<ClubInvite> _clubInvites;

        public ClubInviteService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _clubInvites = database.GetCollection<ClubInvite>("ClubJnvite");
        }

        public void CreateClubInvite(ClubInvite invite) => _clubInvites.InsertOne(invite);

        //Necessary? Maybe for admin, later on
        public ClubInvite GetClubInvite(string id) => _clubInvites.Find(invite => invite.Id == id).FirstOrDefault();

        public List<ClubInvite> GetClubInvites(List<string> ids) => _clubInvites.Find(invite => ids.Contains(invite.Id)).ToList();

        public List<ClubInvite> GetAllClubInvites() => _clubInvites.Find(invites => true).ToList();

        public void DeleteClubInvite(string id) => _clubInvites.DeleteOne(id);

    }
}
