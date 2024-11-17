using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Clubs
{
    public class MembershipRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Player { get; set; }

        public double PlayerHandicap { get; set; }

        public DateTime Date { get; set; }

       // public bool Used { get; set; }

        public MembershipRequest()
        {
            
        }

        public MembershipRequest(string clubId, string playerId, double player_handicap )
        {
            Club = clubId;
            Player = playerId;
            PlayerHandicap = player_handicap;
            Date = DateTime.UtcNow;
        }
    }
}
