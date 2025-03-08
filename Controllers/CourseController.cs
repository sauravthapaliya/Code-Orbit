using Code_Orbit.Data;
using Code_Orbit.Models;
using Microsoft.AspNetCore.Mvc;

namespace Code_Orbit.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CourseController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Course> objCourseList = _db.Courses.ToList();
            return View(objCourseList);
        }

        // Action for the Details page
        public IActionResult Details(int id)
        {
            // Fetch the course by ID
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
    }
}
