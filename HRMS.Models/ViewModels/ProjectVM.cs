using HRMS.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.ViewModels
{
    public class ProjectVM
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
        public string? RateTypeName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public int PriorityId { get; set; }
        public string? PriorityName { get; set; }
        public int TeamId { get; set; }
        public int TeamLeadId { get; set; }
        public string? TeamLeadName { get; set; }
        public string? TeamLeadEmail { get; set; }
        public string? TeamLeadPhone { get; set; }
        public string? TeamLeadProfilePic { get; set; }
        public string? TeamStatusName { get; set; }
        public List<EmployeeVM>? TeamMembers { get; set; }
        public List<int> Employees { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        [DisplayName("Created")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsTeamLead { get; set; }

    }
}
