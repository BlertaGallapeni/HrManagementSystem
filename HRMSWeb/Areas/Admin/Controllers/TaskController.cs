using AutoMapper;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMSWeb.Areas.Admin.Views.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskController(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(int projectId)
        {          
            ViewBag.ProjectId = projectId;
            ViewBag.ProjectName = _unitOfWork.Project.GetFirstOrDefault(x => x.Id == projectId).Name;
            return View();
        }
        public IActionResult GetTasksJson(JqueryDatatableParam param, int projectId)
        {
            var queryTasks = _mapper.Map<IEnumerable<TaskVM>>(_unitOfWork.Task.GetAll(x => x.ProjectId == projectId && x.Deleted != true, includeProperties: "Employee.ApplicationUser.UserThumbnail,Project"));

            var culture = GlobalFunctions.GetCulture(HttpContext);
            int totalRecords = queryTasks.Count();
            queryTasks = queryTasks.Where(pro => string.Concat(pro.Name).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
            if (param.sSortDir_0 == "asc")
                queryTasks = queryTasks.OrderBy(x => x.Name);
            else
                queryTasks = queryTasks.OrderByDescending(x => x.Name);
            int totalRecordsFiltered = queryTasks.Count();

            queryTasks = queryTasks.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = queryTasks
            });
        }
        //GET
        public IActionResult Create(int projectId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.ProjectName = _unitOfWork.Project.GetFirstOrDefault(x => x.Id == projectId).Name;
            return View("Create");
        }
        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskVM model, int projectId)
        {
            if (ModelState.IsValid)
            {
                HRMS.Models.Entities.Task task = _mapper.Map<HRMS.Models.Entities.Task>(model);
                task.ProjectId = projectId;
                _unitOfWork.Task.Add(task);
                
                _unitOfWork.Save();
                TempData["success"] = "Task added successfully";

                return RedirectToAction("Index", new { projectId });

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
            var task = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id, includeProperties: "Status,Employee.ApplicationUser");
            if (task == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<TaskVM>(task);

            return View("Edit", model);
        }

        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskVM model)
        {
            if (ModelState.IsValid)
            {
                var task = _unitOfWork.Task.GetFirstOrDefault(x => x.Id == model.Id);


                _mapper.Map(model, task);
                _unitOfWork.Save();
                TempData["success"] = "Task updated successfully";
                return RedirectToAction("Index", new { projectId = model.ProjectId });
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

            var task = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id && u.Deleted != true);

            if (task == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<TaskVM>(task);


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
            var task = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id);
            if (task == null)
            {
                NotFound();
            }
            _unitOfWork.Task.Delete(task);
            _unitOfWork.Save();
            TempData["success"] = "Task deleted successfully";

            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }


        [HttpGet]
        public IActionResult GetStatus(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.Status.GetStatusListByLang(culture, q, "T"));

        }
        [HttpGet]
        public IActionResult GetTeamMembers(string q, int projectId)
        {
            var members = _mapper.Map<ProjectVM>(_unitOfWork.Project.GetFirstOrDefault(x => x.Id == projectId, includeProperties: "Team.TeamMembers.Employee.ApplicationUser")).TeamMembers;

            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                members = members.Where(x => x.ApplicationUserVM.FirstName.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(members);
        }
    }
}