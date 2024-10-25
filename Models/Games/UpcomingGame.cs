using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Games
{
    public class UpcomingGame
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Course { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Organiser { get; set; }

        public int AvailableSlots { get; set; }

        public DateTime? Date { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Players { get; set; }

        public bool HasSpace { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }
    }
}
