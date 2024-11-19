using FairwayAPI.Models.Courses;
using FairwayAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace FairwayAPI.Tests
{
    public class CourseTests
    {
        
        // testing to see if app is able to retrieve courses
        [Fact]
        public void DoesCourseExist()
        {
            // Arrange
            DotNetEnv.Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("FairwayConnectionString");
            CourseService courseService = new CourseService(connectionString);
            string courseId = "6448275385f1efa1a8edf8e6";
            Course retrievedCourse = new Course();

            // Act
            retrievedCourse = courseService.GetCourse(courseId);

            // Assert
            Assert.NotNull(retrievedCourse);

        }
    }
}
