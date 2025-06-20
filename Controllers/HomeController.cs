using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.data.models;
using mvc.Data;
using mvc.Models;
using mvc.ViewModels;

namespace mvc.Controllers
{
    public class HomeController : BaseController
    {

        private readonly UserManager<Users> _userManager;
        public HomeController(UserManager<Users> userManager) : base(userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Index"] = Index;
            return View();
        }

        public IActionResult History()
        {
            ViewData["Title"] = "History"; // Set the ViewData["Title"]
            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Title"] = "AccessDenied";
            return View();
        }
    }
}