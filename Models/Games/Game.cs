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
        public string? Organiser { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Course { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? Date { get; set; }

        public GameScorecard? Scorecard { get; set; }

        public string[]? Players { get; set; }

        public bool Active { get; set; }

        public GameResult? Result { get; set; }

        public Game(string courseId, DateTime date, string organiser, GameScorecard scorecard, string[] players, GameResult results)
        {
            Course = courseId;
            Date = date;
            Scorecard = scorecard;
            Players = players;
            Result = results;
            Active = false;
            Organiser = organiser;
        }

        public Game()
        {
            
        }
    }
}
