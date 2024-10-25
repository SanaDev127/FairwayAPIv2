using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Games
{
    public class PlayerScorecard
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Player { get; set; }

        public int[]? Strokes { get; set; }

        public int[]? Points { get; set; }



    }
}
