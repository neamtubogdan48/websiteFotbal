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

        // GET: Contact
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return View(contacts);
        }

        // GET: Contact/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // GET: Contact/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("message,userId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                await _contactService.AddContactAsync(contact.userId, contact.message);
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contact/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (id != contact.id)
            {
                return BadRequest("Contact ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                await _contactService.UpdateContactAsync(contact);
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contact/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _contactService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
