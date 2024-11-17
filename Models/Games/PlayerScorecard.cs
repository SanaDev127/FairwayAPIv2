using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Games
{
    public class PlayerScorecard
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? PlayerId { get; set; }

        public string? PlayerName { get; set; }

        public int[]? Strokes { get; set; }

        public int[]? Points { get; set; }

        public PlayerScorecard()
        {
            PlayerId = ObjectId.Empty.ToString();
            PlayerName = "";
            Strokes = new int[18];
            Points = new int[18];
        }

        public PlayerScorecard(string playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Strokes = new int[18];
            Points = new int[18];
        }

    }
}
