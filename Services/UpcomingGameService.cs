using FairwayAPI.Models.Games;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class UpcomingGameService
    {
        private readonly IMongoCollection<UpcomingGame> _upcomingGames;
        public UpcomingGameService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _upcomingGames = database.GetCollection<UpcomingGame>("UpcomingGame");
        }

        public void CreateUpcomingGame(UpcomingGame game) => _upcomingGames.InsertOne(game);

        public UpcomingGame GetUpcomingGame(string id) => _upcomingGames.Find(game => game.Id == id).FirstOrDefault();

        public List<UpcomingGame> GetUpcomingGames(List<string> ids) => _upcomingGames.Find(game => ids.Contains(game.Id)).ToList();

        public List<UpcomingGame> GetAllUpcomingGames() => _upcomingGames.Find(game => true).ToList();

        public void UpdateUpcomingGame(string id, UpcomingGame updatedGame) => _upcomingGames.ReplaceOne(game => game.Id == id, updatedGame);

        public void DeleteUpcomingGame(string id) => _upcomingGames.DeleteOne(game => game.Id == id);

    }
}
