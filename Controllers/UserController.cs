using Code_Orbit.Data;
using Code_Orbit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code_Orbit.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userEnrollments = new List<UserEnrollmentViewModel>();

            foreach (var user in users)
            {
                var enrolledCourses = _db.Enrollments
                    .Where(e => e.UserId == user.Id)
                    .Include(e => e.Course)
                    .Select(e => e.Course.CourseName)
                    .ToList();

                userEnrollments.Add(new UserEnrollmentViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    EnrolledCourses = enrolledCourses
                });
            }

            return View(userEnrollments);
        }
    }
}
