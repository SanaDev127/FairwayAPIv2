namespace FairwayAPI.Models.Clubs
{
    public class TransactionReportRecord
    {
        public string MemberName { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double Balance { get; set; }
        public int NumTransactions { get; set; }

        public TransactionReportRecord(string name, double debit, double credit, int numTransactions)
        {
            MemberName = name;
            Debit = debit;
            Credit = credit;
            Balance = Debit - Credit;
            NumTransactions = numTransactions;

        }

        public TransactionReportRecord()
        {
            
        }
    }
}
