using FairwayAPI.Models;
using FairwayAPI.Models.Games;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class OngoingGameService
    {
        private readonly IMongoCollection<OngoingGame> _ongoingGames;
        private readonly IMongoCollection<User> _users;

        public OngoingGameService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _ongoingGames = database.GetCollection<OngoingGame>("OngoingGame");
            _users = database.GetCollection<User>("Users");
        }

        public void CreateOngoingGame(OngoingGame game) => _ongoingGames.InsertOne(game);

        public OngoingGame GetOngoingGame(string id) => _ongoingGames.Find(game => game.Id == id).FirstOrDefault();

        public List<OngoingGame> GetOngoingGames(List<string> ids) => _ongoingGames.Find(game => ids.Contains(game.Id)).ToList();

        public List<OngoingGame> GetAllOngoingGames() => _ongoingGames.Find(game => true).ToList();

        public void UpdateOngoingGame(string id, OngoingGame ongoingGame) => _ongoingGames.ReplaceOne(ongoinggame => ongoingGame.Id == id, ongoingGame);

        public async void DeleteOngoingGame(string gameId)
        {
            // Removing this game from the user's array of active games
            var players = GetOngoingGame(gameId).Players;
            foreach (var player in players)
            {
                var filter = Builders<User>.Filter
                    .Eq(golfer => golfer.Id, player.Id);
                var update = Builders<User>.Update
                    .Pull(golfer => golfer.Games, gameId);
                await _users.UpdateOneAsync(filter, update);

            }
            _ongoingGames.DeleteOne(gameId);
        }

    }
}
