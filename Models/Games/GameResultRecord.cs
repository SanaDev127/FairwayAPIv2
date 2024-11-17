namespace FairwayAPI.Models.Games
{
    public class GameResultRecord
    {
        public string? PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public int Strokes { get; set; }
        public int Points { get; set; }
        // Over or under par
        public int Score { get; set; }
        public int CourseParTotal { get; set; }

        public GameResultRecord()
        {
            
        }

        public GameResultRecord(string playerId, string playerName, int strokes, int points, int numHoles)
        {
            CourseParTotal = numHoles == 9 ? 36 : 72;
            PlayerId = playerId;
            PlayerName = playerName;
            Strokes = strokes;
            Points = points;
            Score = strokes - CourseParTotal;
        }


    }
}
