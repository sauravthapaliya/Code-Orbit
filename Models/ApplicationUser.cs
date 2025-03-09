// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Code_Orbit.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}