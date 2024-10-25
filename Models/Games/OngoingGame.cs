using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FairwayAPI.Models.Courses;

namespace FairwayAPI.Models.Games
{
    public class OngoingGame
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public Course? Course { get; set; }

        public Hole[]? Holes { get; set; } // IDK I feel like it might make the scorekeeping easier if I have this here, but idk

        public GameScorecard Scorecard { get; set; }

        public User[]? Players { get; set; }

    }
}
