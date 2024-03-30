using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace HRMS.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Personal Number")]
        public string PersonalNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public char? Gender { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int CompanyId { get; set; }
        [ValidateNever]
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual UserThumbnail? UserThumbnail { get; set; }

    }
}
