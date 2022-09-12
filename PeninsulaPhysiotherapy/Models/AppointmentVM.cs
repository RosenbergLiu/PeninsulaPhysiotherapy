using System.ComponentModel.DataAnnotations;
namespace PeninsulaPhysiotherapy.Models
{
    public class AppointmentVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        [Required]
        [Phone]
        public string? Phone { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppDate { get; set; }
        [Required]
        public string? Therapist { get; set; }
        public string? JobType { get; set; }
        public string? JobStatus { get; set; }
        public string? CreatedBy { get; set; }
    }
}
