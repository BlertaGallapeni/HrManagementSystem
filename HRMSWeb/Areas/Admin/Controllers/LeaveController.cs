using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMS.Models;
using HRMSWeb.Helpers;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeaveController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveController(
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // GET
        public ActionResult Index()
        {
            var leavetypes = _unitOfWork.LeaveType.GetAll().ToList();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            var model = new CreateLeaveAllocationVM
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };
            return View(model);
        }

        public ActionResult SetLeave(int id)
        {
            var leaveType = _unitOfWork.LeaveType.GetFirstOrDefault(x=>x.Id==id);
            IList<ApplicationUser> employees = new List<ApplicationUser>();
            if(leaveType.Name.ToLower().Contains("maternity"))
            {
                employees = _userManager.GetUsersInRoleAsync("Employee").Result.Where(x=>x.Gender=='F').ToList();
            }
            else if (leaveType.Name.ToLower().Contains("paternity"))
            {
                employees = _userManager.GetUsersInRoleAsync("Employee").Result.Where(x => x.Gender == 'M').ToList();
            }
            else { 
                employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            }
            foreach (var emp in employees)
            {
                if (_unitOfWork.Leave.CheckAllocation(id, emp.Id))
                {
                    continue;
                }
                var allocation = new LeaveVM
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = _unitOfWork.Employee.GetFirstOrDefault(x => x.ApplicationUserId == emp.Id).Id,
                    LeaveTypeId = id,
                    NumberOfDaysLeft = leaveType.DefaultDays,
                    Period = DateTime.Now.Year
                };
                var leaveallocation = _mapper.Map<Leave>(allocation);
                _unitOfWork.Leave.Create(leaveallocation);
            }
            return RedirectToAction("Index");
        }

        // GET
        public ActionResult Details(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var employee = _mapper.Map<EmployeeVM>(user);
            var allocations = _mapper.Map<List<LeaveVM>>
                (_unitOfWork.Leave.GetLeavesByEmployee(employee.ApplicationUserVM.Id));
            var model = new ViewLeaveAllocationVM
            {
                Employee = employee,
                Leave = allocations
            };
            return View(model);
        }   

        // GET
        public ActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET
        public ActionResult Edit(int id)
        {
            var leaveAllocation = _unitOfWork.Leave.FindById(id);
            var model = _mapper.Map<EditLeaveAllocationVM>(leaveAllocation);
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = _unitOfWork.Leave.FindById(model.Id);
                record.NumberOfDaysLeft = model.NumberOfDaysLeft;
                var isSuccess = _unitOfWork.Leave.UpdateLeave(record);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Error while savng");
                    return View(model);
                }
                return RedirectToAction(nameof(Details), new {id = model.EmployeeId});
            }
            catch
            {
                return View(model);
            }
        }
        // GET
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
     
    }
}
