using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string ProjectID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public double Rate { get; set; }
        public int RateTypeId { get; set; }
        [ValidateNever]
        [ForeignKey("RateTypeId")]
        public RateType RateType { get; set; }
        public int StatusId { get; set; }
        [ValidateNever]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public int PriorityId { get; set; }
        [ValidateNever]
        [ForeignKey("PriorityId")]
        public Priority Priority { get; set; }
        public int TeamId { get; set; }
        [ValidateNever]
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
        public int ClientId { get; set; }
        [ValidateNever]
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [DisplayName("Created")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
