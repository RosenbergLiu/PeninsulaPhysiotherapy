using System.ComponentModel.DataAnnotations;

namespace PeninsulaPhysiotherapy.Models
{
    public class EditUserVM
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
        public EditUserVM()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        
    }
}
