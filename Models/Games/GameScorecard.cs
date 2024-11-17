namespace FairwayAPI.Models.Games
{
    public class GameScorecard
    {
        public int[]? Pars { get; set; }

        public PlayerScorecard[]? PlayerScorecards { get; set; }

        public User[]? Players { get; set; }

        public bool Active { get; set; }

        //Unnecessary probably
       /*
        public GameScorecard(int numPlayers)
        {
            Pars = new int[18]; //Might be a problem later
            Players = new User[numPlayers];
            PlayerScorecards = new PlayerScorecard[numPlayers];
            for (int i = 0; i < numPlayers; i++)
            {
                PlayerScorecards[i] = new PlayerScorecard();
            }
        }*/

        public GameScorecard(int numPlayers, User organiser)
        {
            Pars = new int[18]; //Might be a problem later
            Players = new User[numPlayers];

            PlayerScorecard pScorecard = new PlayerScorecard(organiser.Id, organiser.Name);
            PlayerScorecards = [pScorecard];

        }

        public GameScorecard()
        {
            
        }

    }
}
