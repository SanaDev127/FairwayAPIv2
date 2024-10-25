using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models
{
    public class GameInvite
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? RecipientID { get; set; }

        public bool Accepted { get; set; }

        public DateTime DateSent { get; set; }
    }
}
