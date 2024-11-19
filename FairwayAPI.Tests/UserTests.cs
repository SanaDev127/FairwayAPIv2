using FairwayAPI.Models;
using FairwayAPI.Models.Courses;
using FairwayAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairwayAPI.Tests
{
    public class UserTests
    {
        // testing to see if app is able to retrieve courses
        [Fact]
        public void DoesUserExist()
        {
            // Arrange
            DotNetEnv.Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("FairwayConnectionString");
            UserService userService = new UserService(connectionString);
            string userId = "672f5cce843765e9988738b1";
            User retrievedUser = new User();

            // Act
            retrievedUser = userService.GetUser(userId);

            // Assert
            Assert.NotNull(retrievedUser);

        }
    }
}
