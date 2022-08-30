using System.ComponentModel.DataAnnotations;
public class Job
{
    [Key]
    public int JobID { get; set; }
    public string UserID { get; set; }
    [Required]
    public string CustName { get; set; }


    [DataType(DataType.Date)]
    [Required]
    public DateTime JobDateTime { get; set; }


    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name="Phone Number")]
    public string? CustPhone { get; set; }

    [Required]
    [Display(Name = "Therapist Name")]
    public string TherapistName { get; set; }

    [Required]
    [Display(Name ="Service")]
    public string ServiceName { get; set; }

    public JobStatus JobStatus { get; set; }
}

public enum JobStatus
{
    Submitted,
    Approved,
    Rejected,
    Closed
}
