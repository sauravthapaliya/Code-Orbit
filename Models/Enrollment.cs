using Microsoft.AspNetCore.Identity;

namespace Code_Orbit.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }

        // Navigation properties
        public virtual Course Course { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
