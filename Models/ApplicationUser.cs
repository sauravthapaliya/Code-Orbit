// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace Code_Orbit.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Add additional properties if needed (e.g., FirstName, LastName, etc.)
    }
}
