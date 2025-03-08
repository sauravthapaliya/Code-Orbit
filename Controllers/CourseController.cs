using Code_Orbit.Data;
using Code_Orbit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Code_Orbit.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _db;

        // Constructor to inject ApplicationDbContext
        public CourseController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Action for the Index page to list all courses
        public IActionResult Index()
        {
            List<Course> objCourseList = _db.Courses.ToList();
            return View(objCourseList);
        }

        // Action for the Details page to view course details
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

        // GET: Action to display the Create page
        public IActionResult Create()
        {
            return View();
        }

        // POST: Action to handle the Create functionality
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Add(course);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Action to display the Edit page
        public IActionResult Edit(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Action to handle the Edit functionality
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Courses.Update(course);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Courses.Any(c => c.Id == course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Action to display the Delete page
        public IActionResult Delete(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Action to handle the Delete functionality
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            _db.Courses.Remove(course);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
