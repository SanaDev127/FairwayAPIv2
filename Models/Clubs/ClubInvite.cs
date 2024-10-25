using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Clubs
{
    public class ClubInvite
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime? DateCreated { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }

        public string? Identifier { get; set; }

        public string? Url { get; set; }

        public bool? Used { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GeneratedBy { get; set; }
    }
}
