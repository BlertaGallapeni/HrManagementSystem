using AutoMapper;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMS.Models.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser, ApplicationUserVM>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.UserThumbnail))
            .ReverseMap()
            .ForMember(dest => dest.UserThumbnail, x => x.Ignore())
            .ForMember(x => x.Company, x => x.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestVM>()
                .ReverseMap();
            CreateMap<Leave, LeaveVM>().ReverseMap();
            CreateMap<Leave, EditLeaveAllocationVM>().ReverseMap();

            CreateMap<ApplicationUser, ProfileViewModel>()
                .ForPath(dest => dest.ApplicationUserVM, opt => opt.MapFrom(src=>src))
                .ForPath(dest => dest.ApplicationUserVM.ProfilePicture, opt => opt.MapFrom(src => src.UserThumbnail))
                .ReverseMap()
                .ForPath(dest => dest.UserThumbnail, x => x.Ignore())
                .ForPath(dest => dest.UserThumbnail.Id, opt => opt.MapFrom(src => src.ApplicationUserVM.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUserVM.Email));

            CreateMap<Client, ClientVM>()
                .ForMember(dest=>dest.StatusName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Status.NameAl : x.Status.Name))
                .ForMember(dest => dest.ApplicationUserVM, opt => opt.MapFrom(src => src.ApplicationUser))
                .ForPath(dest => dest.ApplicationUserVM.ProfilePicture, opt => opt.MapFrom(src => src.ApplicationUser.UserThumbnail))
                .ReverseMap()
                .ForMember(x => x.Status, x=>x.Ignore())
                .ForMember(x => x.ApplicationUser, x => x.Ignore());

            CreateMap<Designation, DesignationVM>()
                .ForMember(dest=>dest.DepartmentName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Department.NameAl : x.Department.Name))
                .ReverseMap()
                .ForMember(x => x.Department, x => x.Ignore());


            CreateMap<UserThumbnail, UserThumbnailVM>().ReverseMap();

            CreateMap<Models.Entities.Task, TaskVM>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Status.NameAl : x.Status.Name))
                .ForMember(dest => dest.EmployeeProfilePath, opt => opt.MapFrom(dest => dest.Employee.ApplicationUser.UserThumbnail.Path))
                .ForMember(dest => dest.AssignedToFullName, opt => opt.MapFrom(dest => $"{dest.Employee.ApplicationUser.FirstName} {dest.Employee.ApplicationUser.LastName}"))
                .ReverseMap()
                .ForMember(dest=>dest.Employee, x=>x.Ignore())
                .ForMember(dest=>dest.Status, x=>x.Ignore())
                .ForMember(dest=>dest.Project, x=>x.Ignore());
           
            CreateMap<Models.Entities.Task, TaskEmpVM>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Status.NameAl : x.Status.Name))
                .ReverseMap()
                .ForMember(dest => dest.Employee, x => x.Ignore())
                .ForMember(dest => dest.Status, x => x.Ignore())
                .ForMember(dest => dest.Project, x => x.Ignore());

            CreateMap<Project, ProjectVM>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Status.NameAl : x.Status.Name))
                .ForMember(dest => dest.TeamStatusName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Team.Status.NameAl : x.Team.Status.Name))
                .ForMember(dest => dest.RateTypeName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.RateType.NameAl : x.RateType.Name))
                .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Priority.NameAl : x.Priority.Name))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(x => x.Client.CompanyName))
                .ForMember(dest => dest.TeamLeadId, opt => opt.MapFrom(src => src.Team.TeamLeadId))
                .ForMember(dest => dest.TeamLeadName, opt => opt.MapFrom(dest => $"{dest.Team.Employee.ApplicationUser.FirstName} {dest.Team.Employee.ApplicationUser.LastName}"))
                .ForMember(dest => dest.TeamLeadEmail, opt => opt.MapFrom(dest => dest.Team.Employee.ApplicationUser.Email))
                .ForMember(dest => dest.TeamLeadPhone, opt => opt.MapFrom(dest => dest.Team.Employee.ApplicationUser.PhoneNumber))
                .ForMember(dest => dest.TeamLeadProfilePic, opt => opt.MapFrom(dest => dest.Team.Employee.ApplicationUser.UserThumbnail.Path))
                .ForMember(dest => dest.TeamMembers, opt => opt.MapFrom(src => src.Team.TeamMembers.Select(x => x.Employee).ToList()))
                .ReverseMap()
                .ForMember(x => x.Status, x => x.Ignore())
                .ForMember(x => x.Priority, x => x.Ignore())
                .ForMember(x => x.Client, x => x.Ignore())
                .ForMember(x=>x.Team, x => x.Ignore())
                .ForMember(x => x.RateType, x => x.Ignore());
                

            CreateMap<Employee, EmployeeVM>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Department.NameAl : x.Department.Name))
                .ForMember(dest => dest.DesignationName, opt => opt.MapFrom(x => Thread.CurrentThread.CurrentUICulture.Name == SD.Lang_AL ? x.Designation.NameAl : x.Designation.Name))
                .ForMember(dest => dest.ApplicationUserVM, opt => opt.MapFrom(src => src.ApplicationUser))
                .ForPath(dest => dest.ApplicationUserVM.ProfilePicture, opt => opt.MapFrom(src => src.ApplicationUser.UserThumbnail))
                .ReverseMap()
                .ForMember(x => x.Designation, x => x.Ignore())
                .ForMember(x => x.Department, x => x.Ignore())
                .ForMember(x => x.ApplicationUser, x => x.Ignore());

            CreateMap<Department, DepartmentVM>()
                .ReverseMap();
        }
    }
}
