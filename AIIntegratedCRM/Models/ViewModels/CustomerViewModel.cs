namespace AIIntegratedCRM.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ViewModel for Create/Edit modals. 
    /// We preserve CreatedAt only for Edit; for Create it's set server‐side.
    /// </summary>
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        // Preserved on Edit so we don’t overwrite CreatedAt
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}
