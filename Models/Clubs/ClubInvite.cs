using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FairwayAPI.Utils;

namespace FairwayAPI.Models.Clubs
{
    public class ClubInvite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

       // public DateTime? DateCreated { get; set; } = DateTime.Now;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? Club { get; set; }

        public string? Identifier { get; set; }

        public string? Url { get; set; }

       // public bool? Used { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GeneratedBy { get; set; }

        private readonly string UrlAddress = "";

        public ClubInvite()
        {
            
        }

        public ClubInvite(string clubId, string userId)
        {
            Club = clubId;
            Identifier = Guid.NewGuid().ToString();
            Url = UrlHelper.EncodeUrlParameters($"{UrlAddress}/Auth/InviteLogin?id={Identifier}").ToString();
            //Used = false;
            GeneratedBy = userId;
        }
    }
}
