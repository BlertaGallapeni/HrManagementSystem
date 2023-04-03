using AutoMapper;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMS.Utility;
using HRMSWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area(SD.Area_Admin)]
    public class LeaveTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveTypesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: LeaveTypesController       
        public ActionResult Index()
        {
            var leavetypes = _unitOfWork.LeaveType.GetAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public ActionResult Details(int id)
        {
            if (!_unitOfWork.LeaveType.isExists(id))
            {
                return NotFound();
            }
            var leavetype = _unitOfWork.LeaveType.FindById(id);
            var model = _mapper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<LeaveType>(model);
                leaveType.DateCreated = DateTime.Now;
                var isSuccess = _unitOfWork.LeaveType.Create(leaveType);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong!");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!_unitOfWork.LeaveType.isExists(id))
            {
                return NotFound();
            }
            var leavetype = _unitOfWork.LeaveType.FindById(id);
            var model = _mapper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<LeaveType>(model);
                var isSuccess = _unitOfWork.LeaveType.UpdateLeave(leaveType);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong!");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Delete/5
        public ActionResult Delete(int id)
        {
            var leavetype = _unitOfWork.LeaveType.FindById(id);
            if (leavetype == null)
            {
                return NotFound();
            }
            var isSuccess = _unitOfWork.LeaveType.DeleteLeave(leavetype);
            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LeaveTypeVM model)
        {
            try
            {
                var leavetype = _unitOfWork.LeaveType.FindById(id);
                if (leavetype == null)
                {
                    return NotFound();
                }
                var isSuccess = _unitOfWork.LeaveType.DeleteLeave(leavetype);
                if (!isSuccess)
                {                   
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
        public IActionResult GetLeaveTypesJson(JqueryDatatableParam param)
        {
            List<LeaveTypeVM> leaveList = new List<LeaveTypeVM>();
            var queryLeaves = _mapper.Map<IEnumerable<LeaveTypeVM>>(_unitOfWork.LeaveType.GetAll());

            var culture = GlobalFunctions.GetCulture(HttpContext);
            int totalRecords = queryLeaves.Count();
            queryLeaves = queryLeaves.Where(leave => string.Concat(leave.Name).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
            if (param.sSortDir_0 == "asc")
                queryLeaves = queryLeaves.OrderBy(x => x.Name);
            else
                queryLeaves = queryLeaves.OrderByDescending(x => x.Name);
            int totalRecordsFiltered = queryLeaves.Count();

            leaveList = queryLeaves.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = leaveList
            });
        }
    }
}
