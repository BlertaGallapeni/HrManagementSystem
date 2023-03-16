using AutoMapper;
using HRMS.Utility;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserManagementController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }
        private async Task<List<string>> GetUserRoles(string userId)
        {
            return new List<string>(await _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userId && x.Deleted!=true)));
        }
        
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
           
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
           
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }


        
         public async Task<IActionResult> GetUsersJson(JqueryDatatableParam param)
         {
            var users = _mapper.Map<List<ApplicationUserVM>>(_unitOfWork.ApplicationUser.GetAll(x => x.Deleted != true, includeProperties: "Company,UserThumbnail"));
           
           
            int totalRecords = users.Count();
            users = users.Where(user => string.Concat(user.FirstName,
                user.LastName, user.Email).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
            
            if (param.sSortDir_0 == "asc")
                users = users.OrderBy(x => x.FirstName).ToList();
            else
                users = users.OrderByDescending(x => x.LastName).ToList();
            int totalRecordsFiltered = users.Count();

            users = users.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            foreach (ApplicationUserVM user in users)
            {
                user.Roles = await GetUserRoles(user.Id);
            };

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = users
            });
        }


    }

}

