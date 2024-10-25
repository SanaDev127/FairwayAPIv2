using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace FairwayAPI.Models.Games
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameMaster { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Course { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? Date { get; set; }

        public GameScorecard? Scorecard { get; set; }

        public string[]? Players { get; set; }

        public bool Active { get; set; }
    }
}
