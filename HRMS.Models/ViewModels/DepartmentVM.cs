using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string? EncryptedId { get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        [DisplayName("Emri")]
        public string NameAl { get; set; }
        public bool HasEmployee { get; set; }

    }
}
