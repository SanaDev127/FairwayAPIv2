using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class ClubService
    {
        private readonly IMongoCollection<Club> _clubs;
        
        public ClubService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _clubs = database.GetCollection<Club>("Club");
        }

        public string CreateClub(Club club)
        {
            _clubs.InsertOne(club);
            return club.Id;
        }

        public Club GetClub(string id) => _clubs.Find(club => club.Id == id).FirstOrDefault();

        public List<Club> GetClubs(List<string> ids) => _clubs.Find(club => ids.Contains(club.Id)).ToList();

        public List<Club> GetAllClubs() => _clubs.Find(club => true).ToList();
        // Should have some methods for adding and removing members I guess?
        public void UpdateClub(string id, Club updatedClub) => _clubs.ReplaceOne(club => club.Id == id, updatedClub);

        public void DeleteClub(string id) => _clubs.DeleteOne(club => club.Id == id);

       
    }
}
