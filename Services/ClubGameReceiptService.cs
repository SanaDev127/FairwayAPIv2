using FairwayAPI.Models.Clubs;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class ClubGameReceiptService
    {
        private readonly IMongoCollection<ClubGameReceipts> _clubGameReceipts;

        public ClubGameReceiptService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _clubGameReceipts = database.GetCollection<ClubGameReceipts>("ClubGameReceipts");
        }

        public void CreateClubGameReceipt(ClubGameReceipts clubGameReceipt) => _clubGameReceipts.InsertOne(clubGameReceipt);

        public ClubGameReceipts GetClubGameReceipt(string id) => _clubGameReceipts.Find(r => r.Id == id).FirstOrDefault();

        public ClubGameReceipts GetClubGameReceiptByGameID(string gameId) => _clubGameReceipts.Find(r => r.GameId == gameId).FirstOrDefault();

        public List<ClubGameReceipts> GetClubGameReceiptsByClubId(string clubId) => _clubGameReceipts.Find(r => r.ClubId == clubId).ToList();

        public List<ClubGameReceipts> GetAllClubGameReceipts() => _clubGameReceipts.Find(r => true).ToList();

        public void UpdateClubGameReceipt(string receiptId, ClubGameReceipts receipt) => _clubGameReceipts.ReplaceOne(r => r.Id == receiptId, receipt);

        public void DeleteClubGameReceipt(string id) => _clubGameReceipts.DeleteOne(receipt => receipt.Id == id);
      
    }
}
