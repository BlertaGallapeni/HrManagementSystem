using AutoMapper;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMSWeb.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DesignationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DesignationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            IEnumerable<DesignationVM> designations =_mapper.Map<IEnumerable<DesignationVM>>( _unitOfWork.Designation.GetAll(x=>x.Deleted!=true,includeProperties:"Department"));
            return View(designations);
        }

        //GET
        public IActionResult Create()
        {
            return View("Create");
        }
        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DesignationVM model)
        {
            if (ModelState.IsValid)
            {
                var dep = _unitOfWork.Designation.GetFirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower());
                if (dep != null)
                {
                    ModelState.AddModelError("name", "This Designation already exists.");
                }
                else
                {
                    Designation designation = _mapper.Map<Designation>(model);
                    _unitOfWork.Designation.Add(designation);
                    _unitOfWork.Save();
                    TempData["success"] = "Designation added successfully";

                    return RedirectToAction("Index");
                }
              
            }

            return View("Create", model);
        }
        
        // GET
        public IActionResult Edit(string? id)
        {
            if (id != null)
            {
                id = EncryptionHelper.Decrypt(id);
            }
            if (id == "" || id == null || int.Parse(id) == 0)
            {
                return NotFound();
            }
            var dep = _unitOfWork.Designation.GetFirstOrDefault(u => u.Id == int.Parse(id), includeProperties: "Department");
            if (dep == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<DesignationVM>(dep);
            model.EncryptedId = EncryptionHelper.Encrypt(id);

            return View("Edit", model);
        }

        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DesignationVM model)
        {

            if (ModelState.IsValid)
            {
                model.Id = int.Parse(EncryptionHelper.Decrypt(model.EncryptedId));
                var obj = _mapper.Map<Designation>(model);
                
                _unitOfWork.Designation.Update(obj);
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

            var dep = _unitOfWork.Designation.GetFirstOrDefault(u => u.Id == int.Parse(id));

            if (dep == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<DesignationVM>(dep);
            model.EncryptedId = EncryptionHelper.Encrypt(id);
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
            if (_unitOfWork.Designation.HasEmployee(int.Parse(id)))
            {
                TempData["warning"] = "Can not be deleted! There are employees populated with this designation.";

                return RedirectToAction("Index");
            }
            var obj = _unitOfWork.Designation.GetFirstOrDefault(u => u.Id == int.Parse(id));
            if (obj == null)
            {
                NotFound();
            }
            _unitOfWork.Designation.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Designation deleted successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetDesignation(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.Designation.GetDesignationListByLang(culture, q));

        }
        public IActionResult GetDesignationsJson(JqueryDatatableParam param)
        {
            List<DesignationVM> designationsList = new List<DesignationVM>();
            var queryDesignations = _unitOfWork.Designation.GetDesignations().Select(e => {
                e.EncryptedId = EncryptionHelper.Encrypt(e.Id.ToString());
                return e;
            }); 
            int totalRecords = queryDesignations.Count();
            queryDesignations = queryDesignations.Where(d => string.Concat(d.Name,d.DepartmentName,d.Id).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
            if (param.sSortDir_0 == "asc")
                queryDesignations = queryDesignations.OrderBy(x => x.DepartmentName);
            else
                queryDesignations = queryDesignations.OrderByDescending(x => x.DepartmentName);

            int totalRecordsFiltered = queryDesignations.Count();
            designationsList = queryDesignations.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = designationsList
            });
        }

    }
}