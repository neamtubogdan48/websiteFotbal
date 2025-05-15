using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;

        public NewsController(UserManager<Users> userManager, INewsService newsService) : base(userManager)
        {
            _newsService = newsService;
        }

        public IActionResult News()
        {
            ViewData["Title"] = "News"; // Set the ViewData["Title"]
            return View();
        }

        public IActionResult NewsArticle()
        {
            ViewData["Title"] = "NewsArticle"; // Set the ViewData["Title"]
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return View(newsList);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();
            return View(news);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("title,description,publishDate")] News news, IFormFile photoPathFile)
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
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/news");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    news.photo = $"/uploads/news/{uniqueFileName}";
                }
            }
            else
            {
                ModelState.AddModelError("photo", "Please upload a photo.");
            }

            if (ModelState.IsValid)
            {
                await _newsService.AddNewsAsync(news);
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();
            return View(news);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,description,publishDate")] News news, IFormFile photoPathFile)
        {
            if (id != news.id)
                return BadRequest();

            var existingNews = await _newsService.GetNewsByIdAsync(id);
            if (existingNews == null)
                return NotFound();

            existingNews.title = news.title;
            existingNews.description = news.description;
            existingNews.publishDate = news.publishDate;

            // Only validate and update photo if a new file is uploaded
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
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/news");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoPathFile.CopyToAsync(stream);
                    }

                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(existingNews.photo))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingNews.photo.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    existingNews.photo = $"/uploads/news/{uniqueFileName}";
                }
            }

            // Debug: Output all model state errors
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"ModelState Error for '{key}': {error.ErrorMessage}");
                    }
                }
                return View(existingNews);
            }

            await _newsService.UpdateNewsAsync(existingNews);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _newsService.DeleteNewsAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
