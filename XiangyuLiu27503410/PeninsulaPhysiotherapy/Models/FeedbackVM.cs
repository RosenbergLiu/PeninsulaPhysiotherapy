using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PeninsulaPhysiotherapy.Models
{
    public class FeedbackVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Rating { get; set; }

        [Display(Name = "Comment")]
        public string? CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public string? CommentBy { get;set; }

    }
}
