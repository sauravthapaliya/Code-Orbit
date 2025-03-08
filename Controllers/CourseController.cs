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
    }
}
