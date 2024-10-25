using MongoDB.Bson.Serialization.Attributes;

namespace FairwayAPI.Models.Courses
{
    public class Hole
    {
        [BsonElement("Hole")]
        public int Number { get; set; }

        public int Par { get; set; }

        [BsonElement("tees")]
        public Tees? Tees { get; set; }

        public int Handicap { get; set; }
    }
}
