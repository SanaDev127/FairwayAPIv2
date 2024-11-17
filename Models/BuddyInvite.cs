using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Utils;

namespace FairwayAPI.Models
{
    // Going to be exactly like the club invite with the link being generated and so on...
    public class BuddyInvite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Sender { get; set; }

        //public DateTime DateCreated { get; set; } = DateTime.Now;

        public string? Identifier { get; set; }

        public string? Url { get; set; }

        //public bool? Used { get; set; }

        public BuddyInvite(string senderId)
        {
            Sender = senderId;
            Identifier = Guid.NewGuid().ToString();
            Url = UrlHelper.EncodeUrlParameters($"https://fairway/Invites/Buddy?id={Identifier}").ToString();
            //Used = false;
            
        }

    }
}
