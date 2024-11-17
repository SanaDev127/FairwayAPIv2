using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models
{
    public class FriendshipRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? RequesterId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? RecipientId { get; set; }

       // public DateTime Date { get; set; }

        //public bool Used { get; set; }

        public FriendshipRequest()
        {

        }

        public FriendshipRequest(string requesterId, string recipientId)
        {
            RequesterId = requesterId;
            RecipientId = recipientId;
            //Date = DateTime.UtcNow;
        }
    
    }
}
