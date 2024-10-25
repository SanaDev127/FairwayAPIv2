using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FairwayAPI.Models.Courses
{
    public class Course
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("likes")]
        public string[]? Likes { get; set; }

        [BsonElement("name")]
        public string? CourseName { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("website")]
        public string? Website { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("city")]
        public string? City { get; set; }

        [BsonElement("state")]
        public string? State { get; set; }

        [BsonElement("zip")]
        public string? Zip { get; set; }

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("coordinates")]
        public string? Coordinates { get; set; }

        [BsonElement("holes")]
        public int NumHoles { get; set; }

        [BsonElement("lengthFormat")]
        public string? LengthFormat { get; set; }

        [BsonElement("greenGrass")]
        public string? GreenGrass { get; set; }

        [BsonElement("fairwayGrass")]
        public string? FairwayGrass { get; set; }

        [BsonElement("scorecard")]
        public Hole[]? Scorecard { get; set; }

        [BsonElement("teeBoxes")]
        public TeeBox[]? TeeBoxes { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("__v")]
        public int? __v { get; set; }

    }
}
