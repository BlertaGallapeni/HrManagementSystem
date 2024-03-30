using HRMS.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.ViewModels
{
    public class TaskVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public int AssignedTo { get; set; }
        public string? AssignedToFullName { get; set; }

        public string? EmployeeProfilePath { get; set; }
    }
}
