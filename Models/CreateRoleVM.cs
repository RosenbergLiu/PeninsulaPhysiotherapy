using System.ComponentModel.DataAnnotations;
namespace PeninsulaPhysiotherapy.Models
{
    public class CreateRoleVM
    {
        [Required]
        [StringLength(10)]
        public string? RoleName { get; set; }
    }
}
