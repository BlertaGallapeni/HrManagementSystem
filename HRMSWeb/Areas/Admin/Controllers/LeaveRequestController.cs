using AutoMapper;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMSWeb.Helpers;
using HRMSWeb.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace HRMSWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeaveRequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<LeaveRequestsHub> _hub;

        public LeaveRequestController(
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IHubContext<LeaveRequestsHub> hub)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _hub = hub;
        }
        // GET
        public ActionResult Index()
        {
            var leaveRequests = _unitOfWork.LeaveRequest.GetAll(includeProperties: "RequestingEmployee.ApplicationUser,ApprovedBy,LeaveType");
            var leaveRequestsModel = _mapper.Map<List<LeaveRequestVM>>(leaveRequests);
            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequestsModel.Count,
                ApprovedRequests = leaveRequestsModel.Count(q => q.Approved == true),
                PendingRequests = leaveRequestsModel.Count(q => q.Approved == null),
                RejectedRequests = leaveRequestsModel.Count(q => q.Approved == false),
                LeaveRequests = leaveRequestsModel
            };
            return View(model); 
        }

        public ActionResult MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeId = employee.Id;
            var employeeAllocations = _unitOfWork.Leave.GetLeavesByEmployee(employeeId);

            var employeeAllocationsModel = _mapper.Map<List<LeaveVM>>(employeeAllocations);

            return View(employeeAllocationsModel);
        } 

        // GET
        public ActionResult Details(int id)
        {
            var leaveRequest = _unitOfWork.LeaveRequest.GetFirstOrDefault(x=>x.Id==id,includeProperties: "RequestingEmployee.ApplicationUser,ApprovedBy,LeaveType");
            var model = _mapper.Map<LeaveRequestVM>(leaveRequest);
            return View(model);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _unitOfWork.LeaveRequest.FindById(id);
                var employeeid = leaveRequest.RequestingEmployee.ApplicationUserId;
                var leaveTypeid = leaveRequest.LeaveTypeId;
                var allocation = _unitOfWork.Leave.GetLeavesByEmployeeAndType(employeeid,leaveTypeid);
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                allocation.NumberOfDaysLeft = allocation.NumberOfDaysLeft - daysRequested;
                
                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                _unitOfWork.LeaveRequest.UpdateLeave(leaveRequest);
                _unitOfWork.Leave.UpdateLeave(allocation);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _unitOfWork.LeaveRequest.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                _unitOfWork.LeaveRequest.UpdateLeave(leaveRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET
        public ActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes = _unitOfWork.LeaveType.FindAll();
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypeItems;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start date cannot be further in the future than the end date");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                var emp = _unitOfWork.Employee.GetFirstOrDefault(x=>x.ApplicationUserId==employee.Id);
                var allocation = _unitOfWork.Leave.GetLeavesByEmployeeAndType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int)(endDate - startDate).TotalDays;

                if (daysRequested > allocation.NumberOfDaysLeft) 
                {
                    ModelState.AddModelError("", "You do not have sufficient days for this request!");
                    return View(model);
                }

                var leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmployeeId = emp.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId,
                    RequestComments = model.RequestComments
                };
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                var isSuccess = _unitOfWork.LeaveRequest.Create(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong with submitting your record");
                    return View(model);
                }
               // _hub.Clients.All.SendAsync("LeaveRequest");
                return RedirectToAction("MyLeave");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        public ActionResult CancelRequest(int id)
        {
            var leaveRequest = _unitOfWork.LeaveRequest.FindById(id);
            leaveRequest.Cancelled = true;
            _unitOfWork.LeaveRequest.UpdateLeave(leaveRequest);
            return RedirectToAction("MyLeave");
        }

        // GET
        public ActionResult Edit()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
        public ActionResult Delete()
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
        public IActionResult GetLeaveRequestsJson(JqueryDatatableParam param)
        {
            List<LeaveRequestVM> leaveList = new List<LeaveRequestVM>();
            var queryLeaves = _mapper.Map<IEnumerable<LeaveRequestVM>>(_unitOfWork.LeaveRequest.GetAll(includeProperties: "LeaveType,RequestingEmployee.ApplicationUser"));

            var culture = GlobalFunctions.GetCulture(HttpContext);
            int totalRecords = queryLeaves.Count();
            queryLeaves = queryLeaves.Where(leave => string.Concat(leave.DateRequested).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
            switch (param.iSortCol_0)
            {
                case 0:
                    queryLeaves = param.sSortDir_0 == "asc" ? queryLeaves.OrderBy(x => x.RequestingEmployee.ApplicationUserVM.FullName) : queryLeaves.OrderByDescending(x => x.RequestingEmployee.ApplicationUserVM.FullName);
                    break;
                case 1:
                    queryLeaves = param.sSortDir_0 == "asc" ? queryLeaves.OrderBy(x => x.StartDate) : queryLeaves.OrderByDescending(x => x.StartDate);
                    break;
                case 4:
                    queryLeaves = param.sSortDir_0 == "asc" ? queryLeaves.OrderBy(x => x.DateRequested) : queryLeaves.OrderByDescending(x => x.DateRequested);
                    break;
                case 5:
                    queryLeaves = param.sSortDir_0 == "asc" ? queryLeaves.OrderBy(x => x.Approved) : queryLeaves.OrderByDescending(x => x.Approved);
                    break;
            }
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
        public IActionResult GetEmpLeaveRequestsJson(JqueryDatatableParam param)
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeId = employee.Id;
            var employeeRequests = _unitOfWork.LeaveRequest.GetLeaveRequestsByEmployee(employeeId);

            var queryLeaves = _mapper.Map<IEnumerable<LeaveRequestVM>>(employeeRequests);

            int totalRecords = queryLeaves.Count();
            queryLeaves = queryLeaves.Where(leave => string.Concat(leave.DateRequested).Contains(param.sSearch ?? "", StringComparison.OrdinalIgnoreCase));
            if (param.sSortDir_0 == "asc")
                queryLeaves = queryLeaves.OrderBy(x => x.DateRequested);
            else
                queryLeaves = queryLeaves.OrderByDescending(x => x.DateRequested);
            int totalRecordsFiltered = queryLeaves.Count();

            queryLeaves = queryLeaves.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = queryLeaves
            });
        }

        [HttpGet]
        public IActionResult GetAllLeaveTypes(string q)
        {
            var leaveTypes = _unitOfWork.LeaveType.GetAll();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                leaveTypes = leaveTypes.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            var type = _mapper.Map<IEnumerable<LeaveTypeVM>>(leaveTypes);
            return Json(type);
        }
    }
}
