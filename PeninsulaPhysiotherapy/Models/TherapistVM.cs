﻿using System.ComponentModel.DataAnnotations;
namespace PeninsulaPhysiotherapy.Models
{
    public class TherapistVM
    {
        [Key]
        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Display(Name = "Degree or Title")]
        public string? Level { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }
}