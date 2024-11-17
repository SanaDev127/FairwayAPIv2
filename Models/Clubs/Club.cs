using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Clubs
{
    public class Club
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]

        public string? Creator { get; set; }

        public string? Name { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Members { get; set; }
       // public string[]? Games { get; set; }
        //public string[]? UpcomingGames { get; set; }

        public Club()
        {
            
        }

        public Club(string clubName, string creatorID)
        {
            Name = clubName;
            Creator = creatorID;
            Members = [];
           // Games = [];
            //UpcomingGames = [];
        }
    }
}
