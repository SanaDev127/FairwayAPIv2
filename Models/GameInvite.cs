using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models
{
    public class GameInvite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? RecipientID { get; set; }

        //public bool Accepted { get; set; }

        //public DateTime DateSent { get; set; } = DateTime.UtcNow;

        public GameInvite(string game, string recipient)
        {
            GameID = game;
            RecipientID = recipient;
        }

        public GameInvite()
        {
            
        }
    }
}
