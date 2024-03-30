using AutoMapper;
using HRMS.Utility;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;


        public DbInitializer(
            AppDbContext db, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            { 

            }


            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Client)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

                _db.Companies.AddAsync(new Company
                {
                    Name = "TECH",
                    State = "Kosovo",
                    City = "Prishtina",
                    PostalCode = "10000",
                    StreetAddress = "Musine Kokalari street, nr. 65",
                    PhoneNumber = "+41 (0) 44 745 17 77",
                    Website = "https://comitas.com/en/home/"
                    
                }).GetAwaiter().GetResult();
                _db.SaveChanges();

                _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "gallapeniblerta@gmail.com",
                Email = "gallapeniblerta@gmail.com",
                FirstName = "Blerta",
                LastName = "Gallapeni",
                Gender = 'F',
                PhoneNumber = "049010101",
                PersonalNumber = "111222333444",
                CompanyId = 1

            }, "Admin+2023").GetAwaiter().GetResult();

             ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u=>u.Email == "gallapeniblerta@gmail.com");

            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
           }
        return;
       }
    }
}
