using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(UserManager<Users> userManager, IScheduleService scheduleService) : base(userManager)
        {
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Schedule()
        {
            ViewData["Title"] = "Schedule"; // Set the ViewData["Title"]
            var schedules = await _scheduleService.GetAllAsync();
            return View(schedules ?? new List<Schedule>());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleService.GetAllAsync();
            return View(schedules);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null) return NotFound();
            return View(schedule);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("homeTeam,awayTeam,stadium,matchDate,matchTime,result")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                schedule.matchDate = DateTime.SpecifyKind(schedule.matchDate, DateTimeKind.Utc);

                await _scheduleService.AddAsync(schedule);
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null) return NotFound();
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("id,homeTeam,awayTeam,stadium,matchDate,matchTime,result")] Schedule schedule)
        {
            if (id != schedule.id) return BadRequest();

            schedule.matchDate = DateTime.SpecifyKind(schedule.matchDate, DateTimeKind.Utc);

            if (ModelState.IsValid)
            {
                await _scheduleService.UpdateAsync(schedule);
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null) return NotFound();
            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null) return NotFound();

            await _scheduleService.DeleteAsync(schedule);
            return RedirectToAction(nameof(Index));
        }
    }
}
