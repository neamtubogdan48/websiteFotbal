using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class SponsorController : BaseController
    {
        private readonly ISponsorService _sponsorService;
        private readonly IWebHostEnvironment _env;

        public SponsorController(UserManager<Users> userManager, ISponsorService sponsorService, IWebHostEnvironment env) : base(userManager)
        {
            _sponsorService = sponsorService;
            _env = env;
        }

        public async Task<IActionResult> Sponsors()
        {
            ViewData["Title"] = "Sponsors";
            var sponsors = await _sponsorService.GetAllAsync();
            return View(sponsors ?? new List<Sponsor>());
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var sponsors = await _sponsorService.GetAllAsync();
            return View(sponsors);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null) return NotFound();
            return View(sponsor);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("name,description,link,photo")] Sponsor sponsor, IFormFile photoPathFile)
        {
            if (photoPathFile != null && photoPathFile.Length > 0)
            {
                const long maxFileSize = 5 * 1024 * 1024;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(photoPathFile.FileName).ToLower();

                if (photoPathFile.Length > maxFileSize)
                    ModelState.AddModelError("photo", "The file size cannot exceed 5MB.");
                if (!allowedExtensions.Contains(fileExtension))
                    ModelState.AddModelError("photo", "Only JPG, JPEG and PNG files are allowed.");

                if (ModelState.IsValid)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/sponsors");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    sponsor.photo = $"/uploads/sponsors/{uniqueFileName}";
                }
            }
            else
            {
                ModelState.AddModelError("photo", "Please upload a photo.");
            }

            if (ModelState.IsValid)
            {
                await _sponsorService.AddAsync(sponsor);
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null) return NotFound();
            return View(sponsor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,link,photo")] Sponsor sponsor, IFormFile photoPathFile)
        {
            var existing = await _sponsorService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (photoPathFile != null && photoPathFile.Length > 0)
            {
                const long maxFileSize = 5 * 1024 * 1024;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(photoPathFile.FileName).ToLower();

                if (photoPathFile.Length > maxFileSize)
                    ModelState.AddModelError("photo", "The file size cannot exceed 5MB.");
                if (!allowedExtensions.Contains(fileExtension))
                    ModelState.AddModelError("photo", "Only JPG, JPEG and PNG files are allowed.");

                if (ModelState.IsValid)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/sponsors");
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
                        var oldFilePath = Path.Combine(_env.WebRootPath, existing.photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    sponsor.photo = $"/uploads/sponsors/{uniqueFileName}";
                }
            }
            else
            {
                sponsor.photo = existing.photo;
            }

            if (ModelState.IsValid)
            {
                sponsor.id = id;
                await _sponsorService.UpdateAsync(sponsor);
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null) return NotFound();
            return View(sponsor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null) return NotFound();

            // Delete photo file
            if (!string.IsNullOrEmpty(sponsor.photo))
            {
                var photoPath = Path.Combine(_env.WebRootPath, sponsor.photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(photoPath)) System.IO.File.Delete(photoPath);
            }

            await _sponsorService.DeleteAsync(sponsor);
            return RedirectToAction(nameof(Index));
        }
    }
}
