using System.ComponentModel.DataAnnotations;
namespace PeninsulaPhysiotherapy.Models
{
    public class AppointmentVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        public DateTime AppDate { get; set; }
        public string Therapist { get; set; }
        public string JobType { get; set; }
    }
}
