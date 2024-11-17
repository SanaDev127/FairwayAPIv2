using FairwayAPI.Models.Games;

namespace FairwayAPI.Models.Leagues
{
    public class LeagueTable
    {
        public string LeagueName { get; set; }
        public DateTime LeagueStartDate { get; set; }
        public int NumParticipants { get; set; }
        // Might need to change to an array
        public List<LeagueTableRecord> Table;

        public LeagueTable(string name, DateTime date, List<LeagueTableRecord> records)
        {
            records = records.OrderByDescending(r => r.Points).ToList();
            LeagueName = name;
            LeagueStartDate = date;
            NumParticipants = records.Count;
            Table = records;
           
        }

    }
}
