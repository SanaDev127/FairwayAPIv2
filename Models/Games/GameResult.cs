namespace FairwayAPI.Models.Games
{
    public class GameResult
    {
        // Add this to each game object. Scores are accessed by player name. Use this when displaying results in app
        public Dictionary<string, GameResultRecord>? Results { get; set; }
        public DetailScorecard? Scorecard { get; set; }

        public GameResult(List<GameResultRecord> results, DetailScorecard scorecard)
        {
            results = results.OrderBy(r => r.Points).ToList();
            Results = new Dictionary<string, GameResultRecord>();
            foreach (var result in results)
            {
                Results.Add(result.PlayerId, result);
            }
            Scorecard = scorecard;
        }

        public GameResult()
        {
            
        }

    }
}
