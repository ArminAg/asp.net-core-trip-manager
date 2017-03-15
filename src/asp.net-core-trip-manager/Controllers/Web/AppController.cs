using asp.net_core_trip_manager.Services;
using asp.net_core_trip_manager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {
            _mailService.SendMail("user@domain.com", viewModel.Email, "From Trip Manager", viewModel.Message);
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
