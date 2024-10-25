using FairwayAPI.Models;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class UserService
    {
       
        private readonly IMongoCollection<User> _users;

        public UserService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _users = database.GetCollection<User>("Users");
        }

        public void CreateUser(User user) => _users.InsertOne(user);

        public User GetUser(string id) => _users.Find(user => user.Id == id).FirstOrDefault();

        public List<User> GetUsers(List<string> ids) => _users.Find(user => ids.Contains(user.Id)).ToList();

        public List<User> GetAllUsers() => _users.Find(user => true).ToList();

        public void UpdateUser(string id, User updatedUser) => _users.ReplaceOne(user=> user.Id == id, updatedUser);

        public void DeleteUser(string id) => _users.DeleteOne(id);
    }
}
