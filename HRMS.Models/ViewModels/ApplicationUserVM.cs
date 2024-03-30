using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRMS.Models.ViewModels
{
    public class ApplicationUserVM
    {
        public string? Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        [Display(Name = "Personal Number")]
        public string PersonalNumber { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public char? Gender { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public List<string>? Roles { get; set; }
        public UserThumbnailVM? ProfilePicture { get; set; }

    } 
}
