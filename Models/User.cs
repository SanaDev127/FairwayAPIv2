using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models
{
    public class User
    {
        public User()
        {
            
        }

        public User(string username, string userId, string email)
        {
            Name = username;
            UserID = userId;
            Email = email;

        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = String.Empty;

        public string? UserID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Clubs { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Games { get; set; }

        public string? HomeClub { get; set; }

       // public string? Password { get; set; }

        public string? Role { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? ActiveGames { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[]? Friends { get; set; }
    }
}
