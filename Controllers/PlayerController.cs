using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;

namespace mvc.Controllers
{
    public class PlayerController : BaseController
    {
        private readonly IPlayerService _playerService;
        private readonly IWebHostEnvironment _env;

        public PlayerController(UserManager<Users> userManager, IPlayerService playerService, IWebHostEnvironment env) : base(userManager)
        {
            _playerService = playerService;
            _env = env;
        }

        public async Task<IActionResult> Players()
        {
            ViewData["Title"] = "Players"; // Set the ViewData["Title"]
            var players = await _playerService.GetAllAsync();
            return View(players ?? new List<Player>());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var players = await _playerService.GetAllAsync();
            return View(players);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var player = await _playerService.GetByIdAsync(id);
            if (player == null) return NotFound();
            return View(player);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("name,number,position,photo")] Player player, IFormFile photoPathFile)
        {
            // Validate file upload
            if (photoPathFile != null && photoPathFile.Length > 0)
            {
                const long maxFileSize = 5 * 1024 * 1024; // 5MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(photoPathFile.FileName).ToLower();

                if (photoPathFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("photo", "The file size cannot exceed 5MB.");
                }
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("photo", "Only JPG, JPEG and PNG files are allowed.");
                }

                if (ModelState.IsValid)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/players");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    player.photo = $"/uploads/players/{uniqueFileName}";
                }
            }
            else
            {
                ModelState.AddModelError("photo", "Please upload a photo.");
            }

            if (ModelState.IsValid)
            {
                await _playerService.AddAsync(player);
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var player = await _playerService.GetByIdAsync(id);
            if (player == null) return NotFound();
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("name,number,position,photo")] Player player, IFormFile photoPathFile)
        {
            var existing = await _playerService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Validate file upload
            if (photoPathFile != null && photoPathFile.Length > 0)
            {
                const long maxFileSize = 5 * 1024 * 1024; // 5MB
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(photoPathFile.FileName).ToLower();

                if (photoPathFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("photo", "The file size cannot exceed 5MB.");
                }
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("photo", "Only JPG, JPEG and PNG files are allowed.");
                }

                if (ModelState.IsValid)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/players");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    // Delete old photo
                    if (!string.IsNullOrEmpty(existing.photo))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.photo.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    player.photo = $"/uploads/players/{uniqueFileName}";
                }
            }
            else
            {
                player.photo = existing.photo;
            }

            if (ModelState.IsValid)
            {
                player.id = id;
                await _playerService.UpdateAsync(player);
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _playerService.GetByIdAsync(id);
            if (player == null) return NotFound();
            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerService.GetByIdAsync(id);
            if (player == null) return NotFound();

            // Delete photo file
            if (!string.IsNullOrEmpty(player.photo))
            {
                var photoPath = Path.Combine(_env.WebRootPath, player.photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(photoPath)) System.IO.File.Delete(photoPath);
            }

            await _playerService.DeleteAsync(player);
            return RedirectToAction(nameof(Index));
        }
    }
}
