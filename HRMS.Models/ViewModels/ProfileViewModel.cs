using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRMS.Models.ViewModels
{
    public class ProfileViewModel
    {
       
        public string? RoleName { get; set; }
       
        public ApplicationUserVM ApplicationUserVM { get; set; }
        public string? ProfilePicturePath { get; set; }
    }
}
