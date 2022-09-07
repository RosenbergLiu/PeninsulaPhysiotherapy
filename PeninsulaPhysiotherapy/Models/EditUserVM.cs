using System.ComponentModel.DataAnnotations;

namespace PeninsulaPhysiotherapy.Models
{
    public class EditUserVM
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
        public EditUserVM()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        
    }
}
