
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMS.Utility;
using HRMSWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Umbraco.Core.Persistence.Repositories;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<RequestLocalizationOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,
                              IOptions<RequestLocalizationOptions> options,
                              IHttpContextAccessor httpContextAccessor,
                              IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var projects = _unitOfWork.Project.GetAll(x => x.Deleted != true);
            var employees = _unitOfWork.Employee.GetAll(x => x.Deleted != true);
            var clients = _unitOfWork.Client.GetAll(x => x.Deleted != true);
            var tasks = _unitOfWork.Task.GetAll(x => x.Deleted != true);

            var model = new DashboardVM
            {
                TotalProjects = projects.Count(),
                TotalClients = clients.Count(),
                TotalEmployees = employees.Count(),
                TotalTasks = tasks.Count(),
            };
            return View(model);
        }
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                var cultureItems = _options.Value.SupportedUICultures.Select(x => x.Name).ToArray();
                if (cultureItems.Any(x => x.Equals(culture)))
                {
                    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = true, Secure = _httpContextAccessor.HttpContext.Request.IsHttps });
                }
                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
     
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}