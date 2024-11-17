using FairwayAPI.Models;
using FairwayAPI.Models.Games;
using MongoDB.Bson;
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

        public DetailScorecard GetOngoingGameScorecard(OngoingGame game)
        {
            DetailScorecard scorecard = new DetailScorecard(game.Holes.Length, game.Players.Length);

            var player_names = game.Players.Select(p => p.Name).ToList();
            for (int i = 0; i < game.Holes.Length; i++)
            {
                scorecard.Holes[i] = game.Holes[i].Number;
                scorecard.Distances[i] = game.Holes[i].Tees.teeBox1.yards;
                scorecard.Pars[i] = game.Holes[i].Par;

            }
            for (int j = 0; j < game.Players.Length; j++)
            {
                scorecard.ScoreDetails[j].PlayerName = game.Players[j].Name;
                scorecard.ScoreDetails[j].Strokes = game.Scorecard.PlayerScorecards[j].Strokes;
                scorecard.ScoreDetails[j].Points = game.Scorecard.PlayerScorecards[j].Points;
                //scorecard.Strokes[j, i] = game.Scorecard.PlayerScorecards[j].Strokes[i];
                //scorecard.Points[j, i] = game.Scorecard.PlayerScorecards[j].Points[i];
            }
            return scorecard;
        }

        public string CreateOngoingGame(OngoingGame game) {
           
            _ongoingGames.InsertOne(game);
            return game.Id;
        }

        public OngoingGame GetOngoingGame(string id) => _ongoingGames.Find(game => game.Id == id).FirstOrDefault();

        public List<OngoingGame> GetOngoingGames(List<string> ids) => _ongoingGames.Find(game => ids.Contains(game.Id)).ToList();

        public List<OngoingGame> GetAllOngoingGames() => _ongoingGames.Find(game => true).ToList();

        public void UpdateOngoingGame(string id, OngoingGame ongoingGame) => _ongoingGames.ReplaceOne(g => g.Id == id, ongoingGame);

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
            _ongoingGames.DeleteOne(game => game.Id == gameId);
        }

    }
}
