using Microsoft.AspNetCore.Mvc;

namespace FairwayAPI.Models.Games
{
    public class DetailScorecard
    {
        public int[]? Holes { get; set; }
        public int[]? Distances { get; set; }
        public int[]? Pars { get; set; }
        public string[]? PlayerNames { get; set; }
        public int[,]? Strokes { get; set; }
        public int[,]? Points { get; set; }

        public DetailScorecard(int numHoles, int numPlayers)
        {
            Holes = new int[numHoles];
            Distances = new int[numHoles];
            Pars = new int[numHoles];
            PlayerNames = new string[numPlayers];
            Strokes = new int[numPlayers, numHoles];
            Points = new int[numPlayers, numHoles];
        }

        public static explicit operator DetailScorecard(ActionResult<DetailScorecard> v)
        {
            throw new NotImplementedException();
        }
    }
}
