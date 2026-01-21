using Microsoft.AspNetCore.Identity;

namespace SkillHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsApproved { get; set; } = false; // Default false, mainly for Providers
    }
}
