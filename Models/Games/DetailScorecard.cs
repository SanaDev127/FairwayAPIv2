using Microsoft.AspNetCore.Mvc;

namespace FairwayAPI.Models.Games
{
    public class DetailScorecard
    {
        public int[]? Holes { get; set; }
        public int[]? Distances { get; set; }
        public int[]? Pars { get; set; }
        public ScorecardRecord[] ScoreDetails { get; set; }
       // public string[]? PlayerNames { get; set; }
        //public int[,]? Strokes { get; set; }
        //public int[,]? Points { get; set; }

        public DetailScorecard(int numHoles, int numPlayers)
        {
            Holes = new int[numHoles];
            Distances = new int[numHoles];
            Pars = new int[numHoles];
            ScoreDetails = new ScorecardRecord[numPlayers];
            for (int i = 0; i < numPlayers; i++)
            {
                ScoreDetails[i] = new ScorecardRecord(numHoles);
            }

            //PlayerNames = new string[numPlayers];
            //Strokes = new int[numPlayers, numHoles];
            //Points = new int[numPlayers, numHoles];
        }

        public DetailScorecard()
        {
            
        }

        public static explicit operator DetailScorecard(ActionResult<DetailScorecard> v)
        {
            throw new NotImplementedException();
        }
    }

   
}
