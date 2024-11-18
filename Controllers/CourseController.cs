using FairwayAPI.Models.Courses;
using FairwayAPI.Models.Inputs;
using FairwayAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FairwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpPost("GetAllCourses")]
        public ActionResult<List<Course>> GetAllCourses()
        {
            List<Course> courses = _courseService.GetAllCourses();
            if (courses == null)
            {
                return NoContent();
            }
          
            return Ok(courses);

        }

        [HttpPost("GetCourse")]
        public ActionResult<List<Course>> GetCourse([FromBody] GetCourseInput input)
        {
           
            Course course = _courseService.GetCourse(input.id);
            if (course == null)
            {
                return NotFound("No course found with that ID");
            }

            return Ok(course);

        }

    }

    public class GetCourseInput
    {
        public string id { get; set; }
    }
}
