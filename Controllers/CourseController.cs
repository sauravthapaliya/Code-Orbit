using Code_Orbit.Data;
using Code_Orbit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Code_Orbit.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public CourseController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // When no user is logged in or when the logged-in user is an Admin,
        // show all courses. For a normal logged-in user, show only enrolled courses.
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var enrolledCourseIds = _db.Enrollments
                                          .Where(e => e.UserId == userId)
                                          .Select(e => e.CourseId)
                                          .ToList();
                var courses = _db.Courses
                                 .Where(c => enrolledCourseIds.Contains(c.Id))
                                 .ToList();
                return View(courses);
            }
            else
            {
                List<Course> courses = _db.Courses.ToList();
                return View(courses);
            }
        }

        public IActionResult Details(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        // ADMIN: Create a new course
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        // ADMIN: Edit an existing course
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _db.Courses.Update(course);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // ADMIN: Delete a course
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();

            _db.Courses.Remove(course);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // --- Enrollment Functionality for Normal Users ---

        // GET: Course/Enroll/5 - Display enrollment confirmation page
        [Authorize]
        public IActionResult Enroll(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingEnrollment = _db.Enrollments.FirstOrDefault(e => e.CourseId == id && e.UserId == userId);
            if (existingEnrollment != null)
            {
                // If already enrolled, redirect to course details (or a My Courses page)
                return RedirectToAction("Details", new { id = id });
            }

            return View(course);
        }

        // POST: Course/EnrollConfirmed/5 - Process enrollment confirmation
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult EnrollConfirmed(int id)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var existingEnrollment = _db.Enrollments.FirstOrDefault(e => e.CourseId == id && e.UserId == userId);
            if (existingEnrollment != null)
            {
                return RedirectToAction("Details", new { id = id });
            }

            Enrollment newEnrollment = new Enrollment
            {
                CourseId = id,
                UserId = userId
            };

            _db.Enrollments.Add(newEnrollment);
            _db.SaveChanges();

            // After enrolling, redirect to the Index which for normal users shows only enrolled courses.
            return RedirectToAction(nameof(Index));
        }

        // GET: Course/Browse - Show courses that the user has not enrolled in yet.
        [Authorize]
        public IActionResult Browse()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var enrolledCourseIds = _db.Enrollments
                                      .Where(e => e.UserId == userId)
                                      .Select(e => e.CourseId)
                                      .ToList();
            var courses = _db.Courses
                             .Where(c => !enrolledCourseIds.Contains(c.Id))
                             .ToList();
            return View(courses);
        }
    }
}
