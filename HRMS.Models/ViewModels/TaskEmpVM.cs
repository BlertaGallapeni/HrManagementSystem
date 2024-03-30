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
    public class TaskEmpVM
    {
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
    }
}
