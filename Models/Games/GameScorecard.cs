namespace FairwayAPI.Models.Games
{
    public class GameScorecard
    {
        public int[]? Pars { get; set; }

        public PlayerScorecard[]? PlayerScorecards { get; set; }

        public User[]? Players { get; set; }

        public bool Active { get; set; }

    }
}
