namespace FairwayAPI.Models.Leagues
{
    public class LeagueTableRecord
    {
        //public int Position {get; set;}
        public string PlayerName {get; set;}
        public int NumGamesPlayed {get; set;}
        public int Points {get; set;}
        public double Handicap {get; set;}

        public LeagueTableRecord(string name, int numGames, int points, double handicap)
        {
            //Position = position;
            PlayerName = name;
            NumGamesPlayed = numGames;
            Points = points;
            Handicap = handicap;
        }

        public LeagueTableRecord()
        {
            
        }


    }
}
