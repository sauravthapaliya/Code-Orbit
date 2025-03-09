namespace Code_Orbit.Models
{
    public class UserEnrollmentViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> EnrolledCourses { get; set; } = new List<string>();
    }
}
