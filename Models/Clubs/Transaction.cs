using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Clubs
{
    public class Transaction
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }

        public DateTime? Date { get; set; }

        public float? Amount { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Member { get; set; }

        public string? Operation { get; set; }

        public string? Reason { get; set; }
    }

}
