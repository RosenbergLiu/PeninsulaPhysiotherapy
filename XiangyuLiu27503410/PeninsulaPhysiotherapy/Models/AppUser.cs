using Microsoft.AspNetCore.Identity;

namespace PeninsulaPhysiotherapy.Models
{
    public class AppUser : IdentityUser
    {
        public bool rated { get; set; }

    }
}
