using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController: Controller
    {
        private readonly TransactionService _transactionService;
        private readonly ClubService _clubService;
        private readonly UserService _userService;
        public TransactionController(TransactionService transactionService, ClubService clubService, UserService userService)
        {
            _clubService = clubService;
            _transactionService = transactionService;
            _userService = userService;
        }

        [HttpGet("GetAllClubTransactions")]
        public ActionResult GetAllClubTransactions(string clubId)
        {
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Club == clubId).ToList();
            return Ok(transactions);
        }

        [HttpGet("GetAllUserTransactions")]
        public ActionResult GetAllUserTransactions(string userId)
        {
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Member == userId).ToList();
            return Ok(transactions);
        }

        [HttpPost("CreateTransaction")]
        public ActionResult CreateTransaction(Transaction transaction)
        {
            _transactionService.CreateTransaction(transaction);
            return Ok();
        }

        [HttpGet("GenerateClubFinancialReport")]
        public ActionResult GenerateClubFinancialReport(string clubId, string startDate = "", string endDate = "")
        {
            Club club = _clubService.GetClub(clubId);
            List<User> members = _userService.GetUsers(club.Members.ToList());
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Club == clubId).ToList();

            if (!startDate.Equals("") && !endDate.Equals(""))
            {
                transactions = transactions.Where(t => t.Date >= DateTime.Parse(startDate) && t.Date <= DateTime.Parse(endDate)).ToList();
            }

            int totalTransactions = transactions.Count;
            List<TransactionReportRecord> records = new List<TransactionReportRecord>();
            double totalDebit = 0;
            double totalCredit = 0;

            foreach (User member in members)
            {
                List<Transaction> memberTransactions = transactions.Where(t => t.Member == member.Id).ToList();
                double debit = 0;
                double credit = 0;
                foreach (Transaction transaction in memberTransactions)
                {
                    if (transaction.Operation.Equals("Debit"))
                    {
                        debit += (double)transaction.Amount;
                    }
                    else if (transaction.Operation.Equals("Credit"))
                    {
                        credit += (double)transaction.Amount;
                    }
                }
                TransactionReportRecord record = new TransactionReportRecord(member.Name, debit, credit, memberTransactions.Count);
                records.Add(record);
                totalDebit += debit;
                totalCredit += credit;
            }

            TransactionReport report = new TransactionReport(totalDebit, totalCredit, records, totalTransactions);
            return Ok(report);
        }

        [HttpGet("GenerateUserFinancialReport")]
        public ActionResult GenerateUserFinancialReport(string userId, string startDate = "", string endDate = "")
        {
            User member = _userService.GetUser(userId);
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Member == userId).ToList();
            if (!startDate.Equals("") && !endDate.Equals(""))
            {
                transactions = transactions.Where(t => t.Date >= DateTime.Parse(startDate) && t.Date <= DateTime.Parse(endDate)).ToList();
            }

            double debit = 0;
            double credit = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Operation.Equals("Debit"))
                {
                    debit += (double)transaction.Amount;
                }
                else if (transaction.Operation.Equals("Credit"))
                {
                    credit += (double)transaction.Amount;
                }
            }
            TransactionReportRecord record = new TransactionReportRecord(member.Name, debit, credit, transactions.Count);
            TransactionReport report = new TransactionReport(record);
            return Ok(report);
        }
    }
}
