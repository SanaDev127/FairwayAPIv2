using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Games
{
    public class LeagueGameReceipt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ClubId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GameId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? LeagueId { get; set; }

        public DateTime? Date { get; set; }

        public LeagueGameReceipt() { }

        public LeagueGameReceipt(string leagueId, string gameId)
        {
            
        }

    }
}
