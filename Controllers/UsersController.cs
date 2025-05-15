using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace mvc.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<Users> userManager, IUserService userService, RoleManager<IdentityRole> roleManager) : base(userManager)
        {
            _userService = userService;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin,Suporter, Vizitator")]
        public async Task<IActionResult> UserAsync()
        {
            ViewData["Title"] = "User";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userWithSubscription = await _userService.GetUserWithSubscriptionAsync(user.Id);
            return View(userWithSubscription);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin,Suporter, Vizitator")]
        public async Task<IActionResult> Settings(string id)
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
        [Authorize(Roles = "Admin, Suporter, Vizitator")]
        public async Task<IActionResult> Settings(string id, [Bind("Id,UserName,Email,PasswordHash,accountType,subscriptionId")] Users user, IFormFile? photoPathFile)
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

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UserName,Email,PasswordHash,accountType,subscriptionId,photoPath")] Users user, IFormFile photoPathFile)
        {
            if (photoPathFile != null && photoPathFile.Length > 0)
            {
                // Validate file size (max 5MB)
                const long maxFileSize = 5 * 1024 * 1024; // 5MB in bytes
                if (photoPathFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("profilePhoto", "The file size cannot exceed 5MB.");
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(photoPathFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("profilePhoto", "Only JPG, JPEG and PNG files are allowed.");
                }

                if (ModelState.IsValid)
                {
                    // Define the path to save the uploaded file
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/users");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    // Save the file path in the profilePhoto property
                    user.photoPath = $"/uploads/users/{uniqueFileName}";
                }
            }
            else
            {
                ModelState.AddModelError("profilePhoto", "Please upload a profile photo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Add the user and assign the accountType as a role
                    await _userService.AddUserAsync(user, user.accountType);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PasswordHash,accountType,subscriptionId,photoPath")] Users user, IFormFile photoPathFile)
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
                    ModelState.AddModelError("photoPathFile", "Only JPG, JPEG and PNG files are allowed.");
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

                    user.photoPath = $"/uploads/users/{uniqueFileName}";
                }
            }
            else
            {
                user.photoPath = existingUser.photoPath;
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                await _userService.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(user);
            }
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Call the service to delete the user
                await _userService.DeleteUserAsync(user);

                Console.WriteLine($"User with ID: {id} deleted successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user with ID: {id}. Exception: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
                return View();
            }
        }
    }
}
