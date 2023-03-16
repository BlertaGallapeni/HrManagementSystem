using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities
{
    public class Designation
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string NameAl { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [ValidateNever]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
