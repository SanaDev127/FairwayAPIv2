using FairwayAPI.Models;
using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Games;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class GameService
    {
        private readonly IMongoCollection<Game> _games;
       
        public GameService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _games = database.GetCollection<Game>("Game");
           
        }

        public string CreateGame(Game game)
        {
            _games.InsertOne(game);
            return game.Id;
        }

        public Game GetGame(string id) => _games.Find(game => game.Id == id).FirstOrDefault();

        public List<Game> GetGames(List<string> ids) => _games.Find(game => ids.Contains(game.Id)).ToList();

        public List<Game> GetUserGames(string id)
        {
           return _games.Find(game => game.Players.Contains(id)).ToList();
        }

        public List<Game> GetAllGames() => _games.Find(game => true).ToList();

        public void UpdateGame(string id, Game updatedGame) => _games.ReplaceOne(game => game.Id == id, updatedGame);

        public void DeleteGame(string id) => _games.DeleteOne(game => game.Id == id);

        /*
        public DetailScorecard GenerateDetailScorecard(Game game, Course course, UserService userService)
        {
           
            DetailScorecard scorecard = new DetailScorecard(course.Scorecard.Length, game.Players.Length);
            for (int i = 0; i < course.Scorecard.Length; i++)
            {
                scorecard.Holes[i] = course.Scorecard[i].Number;
                // Fix model so that it has tees so you can get hole distances
                scorecard.Pars[i] = course.Scorecard[i].Par;
                scorecard.PlayerNames[i] = userService.GetUser(game.Players[i]).Name;

                for (int j = 0; j < game.Players.Length; j++)
                {
                    scorecard.Strokes[j, i] = game.Scorecard.PlayerScorecards[j].Strokes[i];
                    scorecard.Points[j, i] = game.Scorecard.PlayerScorecards[j].Points[i];
                }

            }
            return scorecard;
        }
        */
        /*
        [HttpGet("GetGameWinnerDetails")]
        public Dictionary<string, int> GetGameResults(Game game)
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            foreach (var sc in game.Scorecard.PlayerScorecards)
            {
                var pointsTotal = sc.Points.Sum();
                scores.Add(sc.Player, pointsTotal);
            }
            scores.OrderByDescending(s => s.Value);

            return scores;
        }
        */
        public double GetUserHandicapIndex(string userId, List<Game> userGames, CourseService _courseService)
        {
            if (userGames == null || userGames.Count == 0)
            {
                return 54.0;
            }
            else if (userGames.Count >= 20)
            {
                return CalcHandicap(userGames, userId, 8, _courseService);
            }
            else if (userGames.Count == 19)
            {
                return CalcHandicap(userGames, userId, 7, _courseService);
            }
            else if (userGames.Count == 17 || userGames.Count == 18)
            {
                return CalcHandicap(userGames, userId, 6, _courseService);
            }
            else if (userGames.Count == 15 || userGames.Count == 16)
            {
                return CalcHandicap(userGames, userId, 5, _courseService);
            }
            else if (userGames.Count >= 12 || userGames.Count <= 14)
            {
                return CalcHandicap(userGames, userId, 4, _courseService);
            }
            else if (userGames.Count >= 9 || userGames.Count <= 11)
            {
                return CalcHandicap(userGames, userId, 3, _courseService);
            }
            else if (userGames.Count == 7 || userGames.Count == 8)
            {
                return CalcHandicap(userGames, userId, 2, _courseService);
            }
            else if (userGames.Count == 6)
            {
                return CalcHandicap(userGames, userId, 2, _courseService) - 1;
            }
            else if (userGames.Count == 5)
            {
                return CalcHandicap(userGames, userId, 1, _courseService);
            }
            else if (userGames.Count == 4)
            {
                return CalcHandicap(userGames, userId, 1, _courseService) - 1;
            }
            else if (userGames.Count == 3)
            {
                return CalcHandicap(userGames, userId, 1, _courseService) - 2;
            }
            else
            {
                return 0;
            }
        }


                                                                         //Number of games used to get average
        public double CalcHandicap(List<Game> userGames, string userId, int numCalcGames, CourseService courseService)
        {
            var highlightedGames = userGames
                 .OrderByDescending(g => g.Date)
                 .Take(userGames.Count)
                 .ToList();
            // Assuming score differentials have been calculated
            float[] scores = getUserScores(highlightedGames, userId, courseService)
                .OrderBy(g => g)
                .Take(numCalcGames)
                .ToArray();

            double handicap = scores.Average();
            return handicap;
        }

        // Get a list of score differentials for a single user having passed a number of games they were apart of
        public float[] getUserScores(List<Game> userGames, string userId, CourseService courseService)
        {
            float[] scores = new float[userGames.Count];
            int count = 0;
            foreach (var game in userGames)
            {
                int index = Array.IndexOf(game.Players, userId);
                int[] strokes = game.Scorecard.PlayerScorecards[index].Strokes;
                //int[] points = game.Scorecard.PlayerScorecards[index].Points; // Maybe Unnecessary, idk

                // Getting score differential
                var course = courseService.GetCourse(game.Course);
                scores[count] = (float)((strokes.Sum() - course.TeeBoxes[0].Handicap) * (113 / course.TeeBoxes[0].Slope));
                count++;
            }
            return scores;
        }




    }
}
