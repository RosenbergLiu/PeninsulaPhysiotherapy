using Microsoft.AspNetCore.Identity;

namespace PeninsulaPhysiotherapy.Models
{
    public class AppUser : IdentityUser
    {
        public string? NickName { get; set; }

    }
}
