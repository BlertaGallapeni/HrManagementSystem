using HRMS.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models;

namespace HRMS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;

        public UnitOfWork(AppDbContext db) 
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Company = new CompanyRepository(_db);
            Department = new DepartmentRepository(_db);
            Employee = new EmployeeRepository(_db);
            Designation = new DesignationRepository(_db);
            Client = new ClientRepository(_db);
            Status = new StatusRepository(_db);
            Project = new ProjectRepository(_db);
            Team = new TeamRepository(_db);
            Priority = new PriorityRepository(_db);
            RateType = new RateTypeRepository(_db);
            TeamMember = new TeamMemberRepository(_db);
            Task = new TaskRepository(_db);
            Leave = new LeaveRepository(_db);    
            LeaveRequest = new LeaveRequestRepository(_db);
            LeaveType = new LeaveTypeRepository(_db);
        }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public IDesignationRepository Designation { get; private set; }
        public IClientRepository Client { get; private set; }
        public IStatusRepository Status { get; private set; }
        public IProjectRepository Project { get; private set; }
        public ITeamRepository Team { get; private set; }
        public IPriorityRepository Priority { get; private set; }
        public IRateTypeRepository RateType { get; private set; }
        public ITeamMemberRepository TeamMember { get; private set; }
        public ITaskRepository Task { get; private set; }
        public ILeaveRepository Leave { get; private set; }
        public ILeaveTypeRepository LeaveType { get; private set; }
        public ILeaveRequestRepository LeaveRequest { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
