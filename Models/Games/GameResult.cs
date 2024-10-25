namespace FairwayAPI.Models.Games
{
    public class GameResult
    {
       public Dictionary<string, int> Results { get; set; }
        public DetailScorecard Scorecard { get; set; }

        public GameResult(Dictionary<string, int> scores, DetailScorecard scorecard)
        {
            Results = scores;
            Scorecard = scorecard;
        }

    }
}
