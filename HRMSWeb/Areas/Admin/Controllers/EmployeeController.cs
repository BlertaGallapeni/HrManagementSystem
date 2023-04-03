using AutoMapper;
using HRMS.Utility;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMSWeb.Areas.Identity.Pages.Account;
using HRMSWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging.Signing;
using HRMSWeb.Helpers;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence.Repositories;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]

    [Authorize(Roles = SD.Role_Admin)]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private IFileHelper _fileHelper;


        public EmployeeController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILogger<EmployeeController> logger,
            IMapper mapper,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostEnvironment,
            IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem() { Text = "Female", Value = "F" });
            gender.Add(new SelectListItem() { Text = "Male", Value = "M" });
            ViewBag.Gender = gender;

            var model = new EmployeeVM()
            {
                ApplicationUserVM = new ApplicationUserVM()
                {
                    ProfilePicture = new UserThumbnailVM() { }
                }
            };
            return View("CreateEmp", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.ApplicationUser.CheckExist(model.ApplicationUserVM.Email, model.ApplicationUserVM.PersonalNumber))
                {
                    ModelState.AddModelError("CustomError", "User with this email or personal number exists.");
                    return View("CreateEmp", model);
                }

                var user = _mapper.Map<ApplicationUser>(model.ApplicationUserVM);
                user.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, model.ApplicationUserVM.PersonalNumber);
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploadPath = "/images/userProfilePic/" + user.Id + "/" + fileName;

                    _fileHelper.SaveFile(FileTypes.Image, file, "userProfilePic", user.Id, (int)Thumbnails.Grid, (int)Thumbnails.Catalog, (int)Thumbnails.ProfilePicture);

                    var findExisting = _unitOfWork.ApplicationUser.GetUserThumbail(user.Id);
                    if (findExisting != null)
                    {
                        _unitOfWork.ApplicationUser.DeleteThumbnail(findExisting);
                    }
                    var variantUpload = new UserThumbnail
                    {
                        Id = user.Id,
                        Name = fileName,
                        Path = uploadPath,
                    };
                    _unitOfWork.ApplicationUser.AddUserThumbnail(variantUpload);
                }



                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Employee);
                    var emp = _mapper.Map<Employee>(model);
                    emp.ApplicationUserId = user.Id;

                    _logger.LogInformation("User created a new account with password.");
                    _unitOfWork.Employee.Add(emp);
                    _unitOfWork.Save();

                    //await _emailSender.SendEmailAsync(model.ApplicationUserVM.Email, "Employee Created",
                    //$"You are registered successfully as employee in HRMS. <br/> Crendetials to log in:<br/> Username: " + model.ApplicationUserVM.Email + "<br/>Password: (Your Personal Number)");
                    TempData["success"] = "Employee added successfully";
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Create", model);
        }


        //GET
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
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem() { Text = "Female", Value = "F" });
            gender.Add(new SelectListItem() { Text = "Male", Value = "M" });
            ViewBag.Gender = gender;
            var emp = _unitOfWork.Employee.GetFirstOrDefault(x => x.Id == int.Parse(id), includeProperties: "ApplicationUser,Department,Designation,ApplicationUser.Company,ApplicationUser.UserThumbnail");
            var model = _mapper.Map<EmployeeVM>(emp);
            model.EncryptedId = EncryptionHelper.Encrypt(id);

            return View("EditEmp", model);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeVM model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEmp", model);
            }
            model.Id = int.Parse(EncryptionHelper.Decrypt(model.EncryptedId));
            var emp = _unitOfWork.Employee.GetFirstOrDefault(x => x.Id == model.Id);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == model.ApplicationUserVM.Id);

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var uploadPath = "/images/userProfilePic/" + emp.ApplicationUserId + "/" + fileName;
                
                _fileHelper.SaveFile(FileTypes.Image, file, "userProfilePic", emp.ApplicationUserId, (int)Thumbnails.Grid, (int)Thumbnails.ProfilePicture);
                var findExisting = _unitOfWork.ApplicationUser.GetUserThumbail(emp.ApplicationUserId);
                if (findExisting != null)
                {
                    _unitOfWork.ApplicationUser.DeleteThumbnail(findExisting);
                }
                

                var variantUpload = new UserThumbnail
                {
                    Id = emp.ApplicationUserId,
                    Name = fileName,
                    Path = uploadPath,
                };
                _unitOfWork.ApplicationUser.AddUserThumbnail(variantUpload);

            }

            _mapper.Map(model.ApplicationUserVM,user);
            _mapper.Map(model, emp);
    
            _unitOfWork.Save();
            TempData["success"] = "Employee updated successfully";

            return RedirectToAction("Index");;
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

            var employee = _unitOfWork.Employee.GetFirstOrDefault(u => u.Id == int.Parse(id) && u.Deleted!=true);
            var model = _mapper.Map<EmployeeVM>(employee);

            if (employee == null)
            {
                return NotFound();
            }
            model.EncryptedId = EncryptionHelper.Encrypt(id);
            return PartialView("_DeleteEmp", model);
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
            var obj = _unitOfWork.Employee.GetFirstOrDefault(u => u.Id == int.Parse(id), includeProperties: "ApplicationUser");
            if (obj == null)
            {
                NotFound();
            }
            _unitOfWork.Employee.Delete(obj);
            _unitOfWork.ApplicationUser.Delete(obj.ApplicationUser);
            _unitOfWork.Save();
            TempData["success"] = "Employee deleted successfully";

            return RedirectToAction("Index");
        }

        public IActionResult GetEmployeesJson(JqueryDatatableParam param)
        {
            List<EmployeeVM> employeesList = new List<EmployeeVM>();
            var queryEmployees = _mapper.Map<IEnumerable<EmployeeVM>>(_unitOfWork.Employee.GetAll(x => x.Deleted != true, includeProperties: "Department,Designation,ApplicationUser,ApplicationUser.Company,ApplicationUser.UserThumbnail")).Select(e => {
                e.EncryptedId = EncryptionHelper.Encrypt(e.Id.ToString());
                return e;
            });
            int totalRecords = queryEmployees.Count();
            queryEmployees = queryEmployees.Where(emp => string.Concat(emp.ApplicationUserVM.FirstName, 
                emp.ApplicationUserVM.LastName, emp.EmployeeID, 
                emp.DepartmentName, emp.ApplicationUserVM.CompanyName).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));

            if (param.sSortDir_0 == "asc")
                queryEmployees = queryEmployees.OrderBy(x => x.EmployeeID);
            else
                queryEmployees = queryEmployees.OrderByDescending(x => x.ApplicationUserVM.LastName);
            int totalRecordsFiltered = queryEmployees.Count();

            employeesList = queryEmployees.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = employeesList
            });
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetCompany(string q)
        {
            var company = _unitOfWork.Company.GetAll();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                company = company.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(company);
        }
        [HttpGet]
        public IActionResult GetDesignation(string q)
        {
            var designations = _unitOfWork.Designation.GetAll(x=>x.Deleted!=true);
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                designations = designations.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(designations);
        }
        
        #endregion
    }
}
