using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.Entities
{
    public class UserThumbnail
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public virtual ApplicationUser IdNavigation { get; set; }

    }
}
