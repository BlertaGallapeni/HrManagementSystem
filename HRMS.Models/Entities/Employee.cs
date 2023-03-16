using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }
        public string? ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [ValidateNever]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [Required]
        public int DesignationId { get; set; }

        [ValidateNever]
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}
