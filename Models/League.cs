using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models
{
    public class League
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }

        public string? Name { get; set; }

        public DateTime StartDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Participants { get; set; }

        public bool Active { get; set; }
    }
}
