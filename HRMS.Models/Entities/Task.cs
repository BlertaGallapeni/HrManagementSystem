using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.Entities
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        [ValidateNever]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public int StatusId { get; set; }
        [ValidateNever]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public int AssignedTo { get; set; }
        [ValidateNever]
        [ForeignKey("AssignedTo")]
        public Employee Employee { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
