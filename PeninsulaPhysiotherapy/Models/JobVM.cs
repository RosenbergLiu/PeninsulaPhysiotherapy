using System.ComponentModel.DataAnnotations;
namespace PeninsulaPhysiotherapy.Models
{
    public class JobVM
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Full Name")]
        public string? CustName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string? CustPhone { get; set; }

        [Display(Name = "Gender")]
        public Genders Gender { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date and Time")]
        public DateTime DateAndTime { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Physiotherapist")]
        public string? Therapist { get; set; }

        [Required]
        [Display(Name = "Appointment Type")]
        public Types JobType { get; set; }
        public JobStatuses JobStatus { get; set; }

        public enum JobStatuses
        {
            Approved,
            Submited,
            Rejected
        }
        public enum Genders
        {
            M,
            F,
            X
        }
        public enum Types
        {
            Standard,
            subsequent,
            miscellaneous
        }
    }
}
