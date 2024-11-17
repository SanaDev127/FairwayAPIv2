using FairwayAPI.Models.Courses;
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
        [HttpGet("GetAllCourses")]
        public ActionResult<List<Course>> GetAllCourses()
        {
            List<Course> courses = _courseService.GetAllCourses();
            if (courses == null)
            {
                return NoContent();
            }
          
            return Ok(courses);

        }

        [HttpGet("GetCourse")]
        public ActionResult<List<Course>> GetCourse(string id)
        {
           
            Course course = _courseService.GetCourse(id);
            if (course == null)
            {
                return NotFound("No course found with that ID");
            }

            return Ok(course);

        }

    }
}
