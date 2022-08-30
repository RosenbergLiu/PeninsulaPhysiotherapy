using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
namespace PeninsulaPhysiotherapy.Models
{
    public class CreateRoleVM
    {
        [Required]
        [StringLength(10)]
        public string? RoleName { get; set; }
    }
}
