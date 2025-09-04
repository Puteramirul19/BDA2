using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BDA.Entities;
using BDA.Identity;
using BDA.ViewModel;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace BDA.Web.Controllers
{
    [Authorize]
    public class UMAController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UMAController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult BankDraftList()
        {
            return View();
        }

        [Authorize(Roles = "Executive, Manager, Head of Zone, Senior Manager")]
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            ViewBag.Fullname = user.FullName;
            return View();
        }

        // Copied from CancellationController.Process (Stage 2)
        [Authorize(Roles = "TGBS Banking, Business Admin")]
        public IActionResult Process()
        {
            return View();
        }

        // Copied from CancellationController.Receive (Stage 3) 
        [Authorize(Roles = "TGBS Reconciliation")]
        public IActionResult Receive()
        {
            return View();
        }

        // Copied from CancellationController.Confirm (Stage 4)
        [Authorize(Roles = "TGBS Reconciliation")]
        public IActionResult Confirm()
        {
            return View();
        }

        // Stage 5 - Complete
        [Authorize(Roles = "TGBS Reconciliation")]
        public IActionResult Complete()
        {
            return View();
        }

        // All the partial view methods from Cancellation
        public IActionResult _ProcessDetails()
        {
            return View();
        }

        public IActionResult _ActionButton()
        {
            return View();
        }

        public IActionResult _ActionHistory()
        {
            return View();
        }

        public IActionResult _StatusBar()
        {
            return View();
        }

        public IActionResult _Document()
        {
            return View();
        }

        public IActionResult _Comments()
        {
            return View();
        }

        public IActionResult _ReceiveDetails()
        {
            return View();
        }

        public IActionResult _CreateDetails()
        {
            return View();
        }
    }

}