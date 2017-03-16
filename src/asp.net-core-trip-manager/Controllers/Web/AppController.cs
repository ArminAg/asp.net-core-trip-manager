using asp.net_core_trip_manager.Models;
using asp.net_core_trip_manager.Persistence;
using asp.net_core_trip_manager.Services;
using asp.net_core_trip_manager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private ITripRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService, IConfigurationRoot config, ITripRepository repository, ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var trips = _repository.GetAllTrips();
                return View(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index Page: {ex.Message}");
                return Redirect("/error");
            }
            
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], viewModel.Email, "From Trip Manager", viewModel.Message);
                ModelState.Clear();
                ViewBag.Message = "Message Sent";
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
