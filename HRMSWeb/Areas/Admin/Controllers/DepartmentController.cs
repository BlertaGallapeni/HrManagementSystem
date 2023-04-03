using AutoMapper;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMSWeb.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using Umbraco.Core;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _db = db;
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
        public IActionResult Create(DepartmentVM model)
        {
            if (ModelState.IsValid)
            {
                var dep = _unitOfWork.Department.GetFirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower());
                if (dep != null)
                {
                    ModelState.AddModelError("name", "This Department already exists.");
                }
                else
                {
                    Department department = _mapper.Map<Department>(model);
                    _unitOfWork.Department.Add(department);
                    _unitOfWork.Save();
                    TempData["success"] = "Department added successfully";

                    return RedirectToAction("Index");
                }
              
            }
            return View("Create");
        }
        //GET
        public IActionResult Edit(string? id)
        {
            if(id != null) 
            { 
            id = EncryptionHelper.Decrypt(id);
            }
            if (id == "" || id == null || int.Parse(id) == 0)
            {
                return NotFound();
            }
            var dep = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == int.Parse(id));
            if (dep == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<DepartmentVM>(dep);
            model.EncryptedId = EncryptionHelper.Encrypt(id);
            return View("Edit", model);
        }

        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentVM model)
        {

            if (ModelState.IsValid)
            {
                model.Id = int.Parse(EncryptionHelper.Decrypt(model.EncryptedId));
                var obj = _mapper.Map<Department>(model);
                
                _unitOfWork.Department.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Department updated successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //GET
        public IActionResult Delete(string? id)
        {
            if (id != null)
            {
                id = EncryptionHelper.Decrypt(id);
            }
            if (id == "" || id == null || int.Parse(id) == 0)
            {
                return NotFound();
            }

            var dep = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == int.Parse(id));

            if (dep == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<DepartmentVM>(dep);

            return PartialView("_Delete", model);
        }

        //POST 

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(string? id)
        {
            if (id != null)
            {
                id = EncryptionHelper.Decrypt(id);
            }
            if (id == "" || id == null || int.Parse(id) == 0)
            {
                return NotFound();
            }

            if (_unitOfWork.Department.HasEmployee(int.Parse(id)))
            {
                TempData["warning"] = "Can not be deleted! This department has employees.";

                return RedirectToAction("Index");
            }
            var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == int.Parse(id));
            if (obj == null)
            {
                NotFound();
            }
            _unitOfWork.Department.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Department deleted successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetDepartment(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.Department.GetDepartmentListByLang(culture, q));

        }

        public IActionResult GetDepartmentsJson(JqueryDatatableParam param)
        {
            List<DepartmentVM> departmentList = new List<DepartmentVM>();
            var queryDepartments = _unitOfWork.Department.GetDepartments().Select(e => {
                e.EncryptedId = EncryptionHelper.Encrypt(e.Id.ToString());
                return e;
            });
            var culture = GlobalFunctions.GetCulture(HttpContext);
            int totalRecords = queryDepartments.Count();
            queryDepartments = queryDepartments.Where(dep => string.Concat(culture=="sq-AL"?dep.NameAl:dep.Name).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
            if (param.sSortDir_0 == "asc")
                queryDepartments = queryDepartments.OrderBy(x => culture == "sq-AL" ? x.NameAl : x.Name);
            else
                queryDepartments = queryDepartments.OrderByDescending(x => culture == "sq-AL" ? x.NameAl : x.Name);
            int totalRecordsFiltered = queryDepartments.Count();
            
            departmentList = queryDepartments.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = departmentList
            });
        } 
    }
}