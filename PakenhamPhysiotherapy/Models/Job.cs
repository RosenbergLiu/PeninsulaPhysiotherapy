using System.ComponentModel.DataAnnotations;
public class Job
{
    public int JobID { get; set; }
    [Required]
    public string CustName { get; set; }


    [DataType(DataType.Date)]
    [Required]
    public DateTime JobDateTime { get; set; }


    [Required]
    [DataType(DataType.PhoneNumber)]
    public string CustPhone { get; set; }


    public string TherapistName { get; set; }
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
