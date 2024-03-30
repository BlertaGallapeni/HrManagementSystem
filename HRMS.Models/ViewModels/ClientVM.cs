using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace HRMS.Models.ViewModels
{
    public class ClientVM
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Client ID")]
        public string ClientID { get; set; }

        [DisplayName("Company Name")]
        public string? CompanyName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public ApplicationUserVM ApplicationUserVM { get; set; }
    }
}
