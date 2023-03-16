using AutoMapper;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMSWeb.Areas.Admin.Views.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using Umbraco.Core.Models.Membership;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Create()
        {
            return View("Create");
        }
        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectVM model)
        {
            if (ModelState.IsValid)
            {
                Team team = new Team();
                team.TeamLeadId = model.TeamLeadId;
                team.StatusId = 3;
                _unitOfWork.Team.Add(team);
                _unitOfWork.Save();
                foreach (var employee in model.Employees)
                {
                    TeamMember teamMember = new TeamMember();
                    teamMember.Team = team;
                    teamMember.Employee = _unitOfWork.Employee.GetFirstOrDefault(x => x.Id == employee);
                    _unitOfWork.TeamMember.Add(teamMember);
                }

                _unitOfWork.Save();
                Project project = _mapper.Map<Project>(model);
                project.Team= team;
                _unitOfWork.Project.Add(project);
                _unitOfWork.Save();
                TempData["success"] = "Project added successfully";

                return RedirectToAction("Index");

            }
            return View("Create");
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var project = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == id, includeProperties: "Client,Priority,Status,RateType,Team.Employee.ApplicationUser,Team.TeamMembers.Employee.ApplicationUser");
            if (project == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ProjectVM>(project);

            return View("Edit", model);
        }

        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectVM model)
        {

            if (ModelState.IsValid)
            {
                var team = _unitOfWork.Team.GetFirstOrDefault(x => x.Id == model.TeamId,includeProperties:"TeamMembers") ;
                team.TeamLeadId = model.TeamLeadId;
                _unitOfWork.Team.Update(team);
                if(!team.TeamMembers.Select(x => x.EmployeeId).Equals(model.Employees)){
                    foreach (var empDel in team.TeamMembers.Select(x => x.EmployeeId).Except(model.Employees))
                    {
                        _unitOfWork.TeamMember.Remove(team.TeamMembers.FirstOrDefault(x => x.EmployeeId == empDel));
                    }
                    foreach (var emp in model.Employees)
                    {                      
                        if (!team.TeamMembers.Select(x => x.EmployeeId).Contains(emp))
                        {
                            TeamMember teamMember= new TeamMember();
                            teamMember.Team=team;
                            teamMember.Employee = _unitOfWork.Employee.GetFirstOrDefault(x => x.Id == emp);
                            _unitOfWork.TeamMember.Add(teamMember); 
                        }                     
                    }           
                }
                var p = _unitOfWork.Project.GetFirstOrDefault(x => x.Id == model.Id);
                _mapper.Map(model,p);
                _unitOfWork.Save();
                TempData["success"] = "Project updated successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var pro = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == id && u.Deleted!=true);

            if (pro == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ProjectVM>(pro);


            return PartialView("_Delete", model);
        }

        //POST 

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var pro = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == id);
            if (pro == null)
            {
                NotFound();
            }
            _unitOfWork.Project.Delete(pro);
            _unitOfWork.Save();
            TempData["success"] = "Project deleted successfully";

            return RedirectToAction("Index");
        }


        public IActionResult GetProjectsJson(JqueryDatatableParam param)
        {
            var userId = _userManager.GetUserId(User);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault((x => x.Id == userId && x.Deleted != true));
            var roleName = _userManager.GetRolesAsync(user).Result.First();
            if (roleName == "Admin")
            {
                var queryProjects = _mapper.Map<IEnumerable<ProjectVM>>(_unitOfWork.Project.GetAll(x => x.Deleted != true, includeProperties: "Priority,Status,RateType,Team.Status,Team.Employee.ApplicationUser.UserThumbnail,Team.TeamMembers.Employee.ApplicationUser.UserThumbnail"));
                var culture = GlobalFunctions.GetCulture(HttpContext);
                int totalRecords = queryProjects.Count();
                queryProjects = queryProjects.Where(pro => string.Concat(pro.Name).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
                if (param.sSortDir_0 == "asc")
                    queryProjects = queryProjects.OrderBy(x => x.Name);
                else
                    queryProjects = queryProjects.OrderByDescending(x => x.Name);
                int totalRecordsFiltered = queryProjects.Count();

                queryProjects = queryProjects.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = queryProjects
                });
            }
            else if (roleName=="Employee")
            {                
                var queryProjects = _mapper.Map<IEnumerable<ProjectVM>>(_unitOfWork.Project.GetAll(x => x.Team.Employee.ApplicationUser.Id==userId &&  x.Deleted != true, includeProperties: "Priority,Status,RateType,Team.Status,Team.Employee.ApplicationUser.UserThumbnail,Team.TeamMembers.Employee.ApplicationUser.UserThumbnail"));
                var culture = GlobalFunctions.GetCulture(HttpContext);
                int totalRecords = queryProjects.Count();
                queryProjects = queryProjects.Where(pro => string.Concat(pro.Name).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
                if (param.sSortDir_0 == "asc")
                    queryProjects = queryProjects.OrderBy(x => x.Name);
                else
                    queryProjects = queryProjects.OrderByDescending(x => x.Name);
                int totalRecordsFiltered = queryProjects.Count();

                queryProjects = queryProjects.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = queryProjects
                });
            }
            else
            {
               
                return Json(null);
            }

            
        }


        public IActionResult AllEmployees(int projectId)
        {
            var members = _mapper.Map<ProjectVM>(_unitOfWork.Project.GetFirstOrDefault(x => x.Id == projectId, includeProperties: "Team.TeamMembers.Employee.ApplicationUser.UserThumbnail")).TeamMembers;

            return PartialView("_TeamMembers", members);
        }
        [HttpGet]
        public IActionResult GetAllEmployees(string q)
        {
            var employees = _unitOfWork.Employee.GetAll(x => x.Deleted != true, includeProperties: "ApplicationUser");
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                employees = employees.Where(x => x.ApplicationUser.FirstName.ToLower().StartsWith(q.ToLower())).ToList();
            }
            var emp = _mapper.Map<IEnumerable<EmployeeVM>>(employees);
            return Json(emp);
        }
        [HttpGet]
        public IActionResult GetPriority(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.Priority.GetPriorityListByLang(culture, q));

        }
        [HttpGet]
        public IActionResult GetRateType(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.RateType.GetRateTypeListByLang(culture, q));

        }
        [HttpGet]
        public IActionResult GetStatus(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.Status.GetStatusListByLang(culture, q, "P"));

        }
    }
}