using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;
using mvc.ViewModels;

namespace mvc.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;
        private readonly UserManager<Users> _userManager;

        public ContactController(IContactService contactService, UserManager<Users> userManager) : base(userManager)
        {
            _contactService = contactService;
            _userManager = userManager;
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Suporter,Vizitator")]
        public async Task<IActionResult> SubmitContact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Contact", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            await _contactService.AddContactAsync(user.Id, model.Message);


            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Contact");
        }
    }
}
