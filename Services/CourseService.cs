using FairwayAPI.Models.Courses;
using MongoDB.Driver;

namespace FairwayAPI.Services
{
    public class CourseService
    {
        private readonly IMongoCollection<Course> _courses;
        public CourseService(string connectionString)
        {
            var mongoDBClient = new MongoClient(connectionString);
            var database = mongoDBClient.GetDatabase("FairwayDB");

            _courses = database.GetCollection<Course>("Course");
        }

        public void CreateCourse(Course course) => _courses.InsertOne(course);

        public Course GetCourse(string id) => _courses.Find(course => course.Id == id).FirstOrDefault();

        public List<Course> GetCourses(List<string> ids) => _courses.Find(course => ids.Contains(course.Id)).ToList();

        public List<Course> GetAllCourses() => _courses.Find(course => true).ToList();

        public void UpdateCourse(string id, Course updatedCourse) => _courses.ReplaceOne(course => course.Id == id, updatedCourse);

        public void DeleteCourse(string id) => _courses.DeleteOne(id);
    }
}
