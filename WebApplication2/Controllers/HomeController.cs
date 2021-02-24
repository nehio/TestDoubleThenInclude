using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using static WebApplication2.ApplicationDbContext;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            if (!_context.ApplicationUsers.Any())
            {
                AddUserToAppplication();
            }
            var query = _context.ApplicationUsers
                .Include(s => s.Information)
                .ThenInclude(s => s.PhoneNumbers)
                .ThenInclude(s => s.PhoneNumber) // Include generates empty objects with no Id or Values set
                .FirstOrDefault(s=> s.FirstName=="Name");

            if(!query.Information.PhoneNumbers.Any(s=>s.PhoneNumber.Value != null))
            {
                throw new Exception();
            }
            
            return View();
        }

        private void AddUserToAppplication()
        {

            _context.ApplicationUsers.Add(new ApplicationUser
            {
                FirstName = "Name",
                LastName = "LastName",
                Information = new UserInformation
                {
                    PhoneNumbers = new List<UserInformationPhoneNumber>()
                    {
                        new UserInformationPhoneNumber
                        {
                            PhoneNumber = new PhoneNumber
                            {
                                Value = "+33 06 12 34 56 78"
                            }
                        }
                    }
                }
            });

            _context.SaveChanges();
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
