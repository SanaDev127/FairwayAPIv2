using FairwayAPI.Models.Clubs;
using FairwayAPI.Models.Games;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class LeagueGameReceiptService
    {
        private readonly IMongoCollection<LeagueGameReceipt> _leagueGameReceipts;

        public LeagueGameReceiptService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _leagueGameReceipts = database.GetCollection<LeagueGameReceipt>("LeagueGameReceipt");
        }

        public void CreateLeagueGameReceipt(LeagueGameReceipt leagueGameReceipt) => _leagueGameReceipts.InsertOne(leagueGameReceipt);

        public LeagueGameReceipt GetLeagueGameReceipt(string id) => _leagueGameReceipts.Find(r => r.Id == id).FirstOrDefault();

        public List<LeagueGameReceipt> GetLeagueGameReceipts(string leagueId) => _leagueGameReceipts.Find(r => r.LeagueId == leagueId).ToList();

        public List<LeagueGameReceipt> GetAllLeagueGameReceipts() => _leagueGameReceipts.Find(r => true).ToList();

        public void UpdateLeagueGameReceipt(string receiptId, LeagueGameReceipt receipt) => _leagueGameReceipts.ReplaceOne(r => r.Id == receiptId, receipt);

        public void DeleteLeagueGameReceipt(string id) => _leagueGameReceipts.DeleteOne(receipt => receipt.Id == id);

    }
}
