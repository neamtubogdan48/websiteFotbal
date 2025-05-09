using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using mvc.Models;
using mvc.Services;

namespace mvc.Controllers
{
    public class UserAsyncController : BaseController
    {
        private readonly IUserAsyncService _userAsyncService;
        private readonly UserManager<Users> _userManager;
        private readonly IUserService _userService;

        public UserAsyncController(IUserAsyncService userAsyncService, UserManager<Users> userManager, IUserService userService) : base(userManager)
        {
            _userAsyncService = userAsyncService;
            _userManager = userManager;
            _userService = userService;
        }

        [Authorize(Roles = "Admin,Suporter")]
        public async Task<IActionResult> UserAsync()
        {
            ViewData["Title"] = "User";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userWithSubscription = await _userAsyncService.GetUserWithSubscriptionAsync(user.Id);
            return View(userWithSubscription);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin,Suporter")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Suporter")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PasswordHash,accountType,subscriptionId")] Users user, IFormFile? photoPathFile)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Update only the fields that are changed
            if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existingUser.UserName)
            {
                existingUser.UserName = user.UserName;
            }

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                existingUser.Email = user.Email;
            }

            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash != existingUser.PasswordHash)
            {
                existingUser.PasswordHash = user.PasswordHash;
            }

            // Handle profile photo upload if a new file is provided
            if (photoPathFile != null && photoPathFile.Length > 0)
            {
                const long maxFileSize = 5 * 1024 * 1024; // 5MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(photoPathFile.FileName).ToLower();

                if (photoPathFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("photoPathFile", "The file size cannot exceed 5MB.");
                }

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("photoPathFile", "Only JPG, JPEG, and PNG files are allowed.");
                }

                if (ModelState.IsValid)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/users");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(existingUser.photoPath))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingUser.photoPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    existingUser.photoPath = $"/uploads/users/{uniqueFileName}";
                }
            }

            if (!ModelState.IsValid)
            {
                return View(existingUser);
            }

            try
            {
                await _userService.UpdateUserAsync(existingUser);
                return RedirectToAction(nameof(User));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(existingUser);
            }
        }
    }
}