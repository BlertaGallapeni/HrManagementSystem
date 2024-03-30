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
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public int TeamLeadId { get; set; }
        [ValidateNever]
        [ForeignKey("TeamLeadId")]
        public Employee Employee { get; set; }
        [Required]
    
        [DisplayName("Created")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int StatusId { get; set; }
        [ValidateNever]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }

    }
}
