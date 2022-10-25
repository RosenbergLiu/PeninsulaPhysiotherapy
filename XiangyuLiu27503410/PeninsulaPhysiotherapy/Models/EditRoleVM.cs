using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PeninsulaPhysiotherapy.Models
{
    public class EditRoleVM
    {
        public string? Id { get; set; }

        [Required]
        public string? RoleName {get; set; }
        public List<string> Users { get; set; }
        public EditRoleVM()
        {
            Users = new List<string>();
        }

    }
}
