using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Games;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class FairwayDBService
    {
       
        private readonly IMongoCollection<Club> _clubs;
        private readonly IMongoCollection<ClubInvite> _clubInvites;
        private readonly IMongoCollection<Course> _courses;
        private readonly IMongoCollection<Game> _games;
        private readonly IMongoCollection<GameInvite> _gameInvites;
        private readonly IMongoCollection<League> _leagues;
        private readonly IMongoCollection<MembershipRequest> _membershipRequests;
        private readonly IMongoCollection<OngoingGame> _ongoingGames;
        private readonly IMongoCollection<Transaction> _transactions;
        private readonly IMongoCollection<UpcomingGame> _upcomingGames;
        private readonly IMongoCollection<User> _users;

        public FairwayDBService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _clubs = database.GetCollection<Club>("Club");
            _clubInvites = database.GetCollection<ClubInvite>("ClubJnvite");
            _courses = database.GetCollection<Course>("Course");
            _games = database.GetCollection<Game>("Game");
            _gameInvites = database.GetCollection<GameInvite>("GameInvites");
            _leagues = database.GetCollection<League>("League");
            _membershipRequests = database.GetCollection<MembershipRequest>("MembershipRequest");
            _ongoingGames = database.GetCollection<OngoingGame>("OngoingGame");
            _transactions = database.GetCollection<Transaction>("Transaction");
            _upcomingGames = database.GetCollection<UpcomingGame>("UpcomingGame");
            _users = database.GetCollection<User>("Users");

        }

        public void CreateGame(Game game) => _games.InsertOne(game);
    }
}
