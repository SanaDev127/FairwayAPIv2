using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FairwayAPI.Models.Courses;

namespace FairwayAPI.Models.Games
{
    public class OngoingGame
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Organiser {get; set;}

        public DateTime StartTime { get; set; } = DateTime.Now;

        public string[]? Invitees { get; set; }

        public Course? Course { get; set; }

        // Fill this with course holes when creating game
        public Hole[]? Holes { get; set; }

        // These objects only have relevant player details for the game
        public User[]? Players { get; set; }

        public double[]? PlayerHandicaps { get; set; }

        public GameScorecard? Scorecard { get; set; }

        public OngoingGame(User organiser, Course course)
        {
            Organiser = organiser.Id;
            StartTime = DateTime.Now;
            Course = course;
            Holes = course.Scorecard;
            Players = [organiser];
            PlayerHandicaps = [];
            Invitees = [];
            Scorecard = new GameScorecard(1, organiser);
            //If no invitees, single player
        }

        public OngoingGame(User organiser, string[] invitees, Course course)
        {
            Organiser = organiser.Id;
            StartTime = DateTime.Now;
            Course = course;
            Holes = course.Scorecard;
            Players = [organiser];
            PlayerHandicaps = [];
            Invitees = invitees;
            Scorecard = new GameScorecard(invitees.Length + 1, organiser);
        }

        public OngoingGame()
        {
            Organiser = "";
            Course = new Course();
            Holes = [];
            Players = [];
            PlayerHandicaps = [];
            Invitees = [];
            StartTime = DateTime.Now;
            Scorecard = new GameScorecard();
        }

    }
}
