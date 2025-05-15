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