namespace FairwayAPI.Models.Games
{
    public class ScorecardRecord
    {
        public string? PlayerName { get; set; }
        public int[]? Strokes { get; set; }
        public int[]? Points { get; set; }

        public ScorecardRecord(int numHoles)
        {
            PlayerName = "";
            Strokes = new int[numHoles];
            Points = new int[numHoles];
        }

        public ScorecardRecord(string playerName, int numHoles)
        {
            PlayerName = playerName;
            Strokes = new int[numHoles];
            Points = new int[numHoles];
        }

    }
}
