using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Services;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class SubscriptionCRUDController : BaseController
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionCRUDController(ISubscriptionService subscriptionService, UserManager<Users> userManager) : base(userManager)
        {
            _subscriptionService = subscriptionService;
        }


        // GET: Subscription
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            return View(subscriptions);
        }

        // GET: Subscription/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // GET: Subscription/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("type,expireDate")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                // Ensure expireDate is in UTC
                subscription.expireDate = subscription.expireDate.ToUniversalTime();

                await _subscriptionService.AddSubscriptionAsync(subscription);
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // GET: Subscription/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request.");
            }

            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // POST: Subscription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("id,type,expireDate")] Subscription subscription)
        {
            if (id != subscription.id)
            {
                return BadRequest("Subscription ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                // Ensure expireDate is in UTC
                subscription.expireDate = subscription.expireDate.ToUniversalTime();

                await _subscriptionService.UpdateSubscriptionAsync(subscription);
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }



        // GET: Subscription/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // POST: Subscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _subscriptionService.DeleteSubscriptionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
