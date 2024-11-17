using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Clubs
{
    public class ClubGameReceipts
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ClubId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameId { get; set; }

        public DateTime? Date { get; set; }

        public ClubGameReceipts()
        {
            
        }

        public ClubGameReceipts(string clubId, string gameId, DateTime date)
        {
            ClubId = clubId;
            GameId = gameId;
            Date = date;
        }
    }
}
