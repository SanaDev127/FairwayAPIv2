using MongoDB.Bson.Serialization.Attributes;

namespace FairwayAPI.Models.Courses
{
    public class TeeBox
    {
        [BsonElement("tee")]
        public string? Tee { get; set; }

        [BsonElement("slope")]
        public float? Slope { get; set; }

        [BsonElement("handicap")]
        public float? Handicap { get; set; }
    }
}
