namespace FairwayAPI.Models.Clubs
{
    public class TransactionReport
    { 
        public List<TransactionReportRecord> TransactionRecords { get; set; }
        public double Debit { get; set; } = 0;
        public double Credit { get; set; } = 0;
        public double Balance { get; set; }
        public int NumTransactions { get; set; }

        public TransactionReport(double debit, double credit, List<TransactionReportRecord> records,int numTransactions)
        {
            Debit = debit;
            Credit = credit;
            NumTransactions = numTransactions;
            Balance = Debit - Credit;
            TransactionRecords = records;
        }

        public TransactionReport(TransactionReportRecord record)
        {
            Debit = record.Debit;
            Credit = record.Credit;
            Balance = Debit - Credit;
            NumTransactions = record.NumTransactions;
            TransactionRecords = [record];
        }

        public TransactionReport()
        {
            
        }
    }
}
