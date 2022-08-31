using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeninsulaPhysiotherapy.Models;

namespace PeninsulaPhysiotherapy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PeninsulaPhysiotherapy.Models.JobVM>? JobVM { get; set; }
        public DbSet<PeninsulaPhysiotherapy.Models.TherapistVM>? TherapistVM { get; set; }
    }
}