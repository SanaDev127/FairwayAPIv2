using FairwayAPI.Models;
using FairwayAPI.Models.Clubs;
using FairwayAPI.Models.Inputs;
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

        [HttpPost("GetAllClubTransactions")]
        public ActionResult GetAllClubTransactions([FromBody] ClubIdInput input)
        {
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Club == input.clubId).ToList();
            return Ok(transactions);
        }

        [HttpPost("GetAllUserTransactions")]
        public ActionResult GetAllUserTransactions([FromBody] UserIdInput input)
        {
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Member == input.userId).ToList();
            return Ok(transactions);
        }

        [HttpPost("CreateTransaction")]
        public ActionResult CreateTransaction([FromBody]Transaction transaction)
        {
            _transactionService.CreateTransaction(transaction);
            return Ok();
        }

        [HttpPost("GenerateClubFinancialReport")]
        public ActionResult GenerateClubFinancialReport([FromBody] ClubFinancialReportInput input)
        {
            Club club = _clubService.GetClub(input.clubId);
            List<User> members = _userService.GetUsers(club.Members.ToList());
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Club == input.clubId).ToList();

            if (!input.startDate.Equals("") && !input.endDate.Equals(""))
            {
                transactions = transactions.Where(t => t.Date >= DateTime.Parse(input.startDate) && t.Date <= DateTime.Parse(input.endDate)).ToList();
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

        [HttpPost("GenerateUserFinancialReport")]
        public ActionResult GenerateUserFinancialReport([FromBody] UserFinancialReportInput input)
        {
            User member = _userService.GetUser(input.userId);
            List<Transaction> transactions = _transactionService.GetAllTransactions().Where(t => t.Member == input.userId).ToList();
            if (!input.startDate.Equals("") && !input.endDate.Equals(""))
            {
                transactions = transactions.Where(t => t.Date >= DateTime.Parse(input.startDate) && t.Date <= DateTime.Parse(input.endDate)).ToList();
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

    public class ClubFinancialReportInput
    {
        public string clubId { get; set; }
        public string startDate { get; set; } = "";
        public string endDate { get; set; } = "";
    }

    public class UserFinancialReportInput
    {
        public string userId { get; set; }
        public string startDate { get; set; } = "";
        public string endDate { get; set; } = "";
    }
}
