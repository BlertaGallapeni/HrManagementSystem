using AutoMapper;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
    
    }
}
