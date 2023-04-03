using HRMS.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.ViewModels
{
    public class DesignationVM
    {
        public int Id { get; set; }
        public string? EncryptedId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Emri")]
        public string NameAl { get; set; }
        public string? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentNameAl { get; set; }
        public bool HasEmployee { get; set; }

    }
}
