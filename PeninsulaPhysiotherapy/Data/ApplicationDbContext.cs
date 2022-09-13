using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeninsulaPhysiotherapy.Models;

namespace PeninsulaPhysiotherapy.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PeninsulaPhysiotherapy.Models.AppointmentVM>? AppointmentVM { get; set; }
    }
}