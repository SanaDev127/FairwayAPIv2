using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models
{
    public class League
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }

        public string? Name { get; set; }

        public DateTime StartDate { get; set; }

        public bool? Active { get; set; }

        public League(string clubId, string name, DateTime startDate)
        {
            Club = clubId;
            Name = name;
            StartDate = startDate;
            Active = true;
        }
        public League()
        {
            
        }
    }
}
