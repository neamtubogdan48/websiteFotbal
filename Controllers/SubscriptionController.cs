using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc.data.models;
using mvc.Models;
using mvc.Services;

namespace mvc.Controllers
{
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly UserManager<Users> _userManager;

        public SubscriptionController(ISubscriptionService subscriptionService, UserManager<Users> userManager)
            : base(userManager)
        {
            _subscriptionService = subscriptionService;
            _userManager = userManager;
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


        public IActionResult Subscription()
        {
            ViewData["Title"] = "Subscription";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Suporter,Vizitator")]
        public async Task<IActionResult> UpdateSubscriptionType(string subscriptionType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Generate a new subscription ID if the current ID is 0
            if (user.subscriptionId == 0)
            {
                var newSubscription = new Subscription
                {
                    type = subscriptionType,
                    expireDate = new DateTime(DateTime.Now.Year, 5, 31, 0, 0, 0, DateTimeKind.Utc)
                };

                // Add the new subscription and get the generated ID
                await _subscriptionService.AddSubscriptionAsync(newSubscription);
                user.subscriptionId = newSubscription.id;

                // Update the user with the new subscription ID
                await _userManager.UpdateAsync(user);
            }
            else
            {
                // Update the existing subscription
                var subscription = new Subscription
                {
                    id = user.subscriptionId,
                    type = subscriptionType,
                    expireDate = new DateTime(DateTime.Now.Year, 5, 31, 0, 0, 0, DateTimeKind.Utc)
                };

                await _subscriptionService.UpdateSubscriptionAsync(subscription);
            }

            return RedirectToAction("User", "Users");
        }
    }
}