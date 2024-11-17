using FairwayAPI.Models;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class GameInviteService
    {
        private readonly IMongoCollection<GameInvite> _gameInvites;

        public GameInviteService(string connectionString)
        {

            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _gameInvites = database.GetCollection<GameInvite>("GameInvites");
        }

        public void CreateGameInvite(GameInvite invite) => _gameInvites.InsertOne(invite);

        public GameInvite GetGameInvite(string id) => _gameInvites.Find(invite => invite.Id == id).FirstOrDefault();

        public List<GameInvite> GetGameInvites(List<string>ids) => _gameInvites.Find(invite => ids.Contains(invite.Id)).ToList();

        public List<GameInvite> GetUserGameInvites(string userId) => _gameInvites.Find(invite => invite.RecipientID == userId).ToList();

        public List<GameInvite> GetAllGameInvites() => _gameInvites.Find(invite => true).ToList();

        public void DeleteGameInvite(string id) => _gameInvites.DeleteOne(invite => invite.Id == id);
    }
}
