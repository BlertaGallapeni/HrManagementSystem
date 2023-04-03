using HRMS.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace HRMS.Models.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string? EncryptedId { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; }
        //[Required]
        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int DesignationId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public ApplicationUserVM ApplicationUserVM { get; set; }
    }
}
