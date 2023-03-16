using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.Entities
{
    public class TeamMember
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }

    }
}
