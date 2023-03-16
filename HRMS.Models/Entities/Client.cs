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
    public class Client
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        [DisplayName("Client ID")]
        public string ClientID { get; set; }
        [DisplayName("Company Name")]
        public string? CompanyName { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int StatusId { get; set; }
        [ValidateNever]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
    }
}
