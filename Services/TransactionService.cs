using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class TransactionService
    {
        private readonly IMongoCollection<Transaction> _transactions;
        public TransactionService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _transactions = database.GetCollection<Transaction>("Transaction");
        }

        public void CreateTransaction(Transaction transaction) => _transactions.InsertOne(transaction);

        public Transaction GetTransaction(string id) => _transactions.Find(transaction => transaction.Id == id).FirstOrDefault();

        public List<Transaction> GetTransactions(List<string> ids) => _transactions.Find(transaction => ids.Contains(transaction.Id)).ToList();

        public void DeleteTransaction(string id) => _transactions.DeleteOne(id);
    }
}
