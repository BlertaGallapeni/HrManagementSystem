using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRMS.Models.ViewModels
{
    public class DashboardVM
    {
        [Display(Name = "Total Number of Projects")]
        public int TotalProjects { get; set; }
        [Display(Name = "Total Number of Employees")]
        public int TotalEmployees { get; set; }
        [Display(Name = "Total Number of Clients")]
        public int TotalClients { get; set; }
        [Display(Name = "Total Number of Tasks")]
        public int TotalTasks { get; set; }
    }
}
