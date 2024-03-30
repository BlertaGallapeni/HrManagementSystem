
using AutoMapper;
using HRMS.Utility;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence.Repositories;
using HRMSWeb.Helpers;
using System.Web.Helpers;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    public class ClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private IFileHelper _fileHelper;
        public ClientController(IUnitOfWork unitOfWork, ILogger<EmployeeController> logger, IMapper mapper, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            var objClientList = _mapper.Map<IEnumerable<ClientVM>>(_unitOfWork.Client.GetAll(x => x.Deleted != true, includeProperties: "ApplicationUser"));
            return View(objClientList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem() { Text = "Female", Value = "F" });
            gender.Add(new SelectListItem() { Text = "Male", Value = "M" });
            ViewBag.Gender = gender;
            var model = new ClientVM()
            {
                ApplicationUserVM = new ApplicationUserVM()
                {
                    ProfilePicture = new UserThumbnailVM() { }
                }
            };
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.ApplicationUser.CheckExist(model.ApplicationUserVM.Email, model.ApplicationUserVM.PersonalNumber))
                {
                    ModelState.AddModelError("CustomError", "User with this email or personal number exists.");
                    return View("Create", model);
                }
            
                var user = _mapper.Map<ApplicationUser>(model.ApplicationUserVM);
            
                user.CompanyId = 1;
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
                    await _userManager.AddToRoleAsync(user, SD.Role_Client);
                    var client = _mapper.Map<Client>(model);
                    client.ApplicationUserId = user.Id;
                    _logger.LogInformation("User created a new account with password.");
                    _unitOfWork.Client.Add(client);
                    _unitOfWork.Save();

                    //await _emailSender.SendEmailAsync(model.ApplicationUserVM.Email, "Employee Created",
                    //$"You are registered successfully as employee in HRMS. <br/> Crendetials to log in:<br/> Username: " + model.ApplicationUserVM.Email + "<br/>Password: (Your Personal Number)");
                    TempData["success"] = "Client added successfully";
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Create", model);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem() { Text = "Female", Value = "F" });
            gender.Add(new SelectListItem() { Text = "Male", Value = "M" });
            ViewBag.Gender = gender;
            var client = _unitOfWork.Client.GetFirstOrDefault(x => x.Id == id, includeProperties: "ApplicationUser,Status,ApplicationUser.UserThumbnail");
            var model = _mapper.Map<ClientVM>(client);

            return View("Edit", model);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ClientVM model, IFormFile? file)
        {

            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            var client = _unitOfWork.Client.GetFirstOrDefault(x => x.Id == model.Id);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == model.ApplicationUserVM.Id);
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var uploadPath = "/images/userProfilePic/" + client.ApplicationUserId + "/" + fileName;

                _fileHelper.SaveFile(FileTypes.Image, file, "userProfilePic", client.ApplicationUserId, (int)Thumbnails.Grid, (int)Thumbnails.ProfilePicture);
                var findExisting = _unitOfWork.ApplicationUser.GetUserThumbail(client.ApplicationUserId);
                if (findExisting != null)
                {
                    _unitOfWork.ApplicationUser.DeleteThumbnail(findExisting);
                }


                var variantUpload = new UserThumbnail
                {
                    Id = client.ApplicationUserId,
                    Name = fileName,
                    Path = uploadPath,
                };
                _unitOfWork.ApplicationUser.AddUserThumbnail(variantUpload);

            }
            model.ApplicationUserVM.CompanyId = 1;
            _mapper.Map(model, client);
            _mapper.Map(model.ApplicationUserVM, user);
            _unitOfWork.Save();
            TempData["success"] = "Client updated successfully";


            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var client = _unitOfWork.Client.GetFirstOrDefault(u => u.Id == id);
            var model = _mapper.Map<ClientVM>(client);

            if (client == null)
            {
                return NotFound();
            }

            return PartialView("_Delete", model);
        }

        //POST 

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Client.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
            if (obj == null)
            {
                NotFound();
            }
            _unitOfWork.Client.Delete(obj);
            _unitOfWork.ApplicationUser.Delete(obj.ApplicationUser);
            _unitOfWork.Save();

            TempData["success"] = "Client deleted successfully";

            return RedirectToAction("Index");

        }

        public IActionResult GetClientsJson(JqueryDatatableParam param)
        
        {
            List<ClientVM> clientsList = new List<ClientVM>();
            var queryClients = _mapper.Map<IEnumerable<ClientVM>>(_unitOfWork.Client.GetAll(x => x.Deleted != true, includeProperties: "ApplicationUser,Status,ApplicationUser.UserThumbnail"));


            int totalRecords = queryClients.Count();
            queryClients = queryClients.Where(cli => string.Concat(
                cli.ApplicationUserVM.FirstName,
                cli.ApplicationUserVM.LastName,
                cli.ClientID,
                cli.CompanyName,
                cli.ApplicationUserVM.Email,
                cli.ApplicationUserVM.PhoneNumber,
                cli.ApplicationUserVM.PersonalNumber).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));

                switch (param.iSortCol_0)
                {
                    case 0:
                        queryClients = param.sSortDir_0 == "asc" ? queryClients.OrderBy(x => x.CompanyName) : queryClients.OrderByDescending(x => x.CompanyName);
                        break;
                    case 1:
                        queryClients = param.sSortDir_0 == "asc" ? queryClients.OrderBy(x => x.ClientID) : queryClients.OrderByDescending(x => x.ClientID);
                        break;
                    case 2:
                        queryClients = param.sSortDir_0 == "asc" ? queryClients.OrderBy(x => x.ApplicationUserVM.FullName) : queryClients.OrderByDescending(x => x.ApplicationUserVM.FullName);
                        break;
                    case 3:
                        queryClients = param.sSortDir_0 == "asc" ? queryClients.OrderBy(x => x.ApplicationUserVM.Email) : queryClients.OrderByDescending(x => x.ApplicationUserVM.Email);
                        break;
                    case 5:
                        queryClients = param.sSortDir_0 == "asc" ? queryClients.OrderBy(x => x.StatusName) : queryClients.OrderByDescending(x => x.StatusName);
                        break;
                }
                
            int totalRecordsFiltered = queryClients.Count();
            clientsList = queryClients.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = clientsList
            });
        }

        [HttpGet]
        public IActionResult GetStatus(string q)
        {
            var culture = GlobalFunctions.GetCulture(HttpContext);
            return Json(_unitOfWork.Status.GetStatusListByLang(culture, q, "C"));
        }

        [HttpGet]
        public IActionResult GetClients(string q)
        {
            var clients = _unitOfWork.Client.GetAll(x => x.Deleted != true);
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                clients = clients.Where(x => x.CompanyName.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(clients);
        }
 
    }
}
