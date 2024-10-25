﻿using FairwayAPI.Models;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class LeagueService
    {
        private readonly IMongoCollection<League> _leagues;
        public LeagueService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _leagues = database.GetCollection<League>("League");
        }

        public void CreateLeague(League league) => _leagues.InsertOne(league);

        public League GetLeague(string id) => _leagues.Find(league => league.Id == id).FirstOrDefault();

        // Maybe for seeing past records
        public List<League> GetLeagues(List<string> ids) => _leagues.Find(league => ids.Contains(league.Id)).ToList();

        // Idk, adding and removing participants, maybe done in the controller and this just replaces. Maybe most operations are done in the controller and these just replace
        public void UpdateLeague(string id, League updatedLeague) => _leagues.ReplaceOne(league => league.Id == id, updatedLeague);

        public void DeleteLeague(string id) => _leagues.DeleteOne(id);
    }
}
