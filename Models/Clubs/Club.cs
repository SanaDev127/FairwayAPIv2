using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Clubs
{
    public class Club
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]

        public string? Creator { get; set; }

        public string? Name { get; set; }

        public DateTime DateCreated { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Members { get; set; }
        public string[]? Games { get; set; }
        public string[]? UpcomingGames { get; set; }
    }
}
