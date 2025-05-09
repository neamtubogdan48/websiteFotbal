document.addEventListener("DOMContentLoaded", () => {
    const buttons = document.querySelectorAll(".buyButton");

    buttons.forEach(button => {
        button.addEventListener("click", async () => {
            const subscriptionType = button.getAttribute("data-subscription-type");

            try {
                const response = await fetch("/Users/UpdateSubscriptionType", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify({ subscriptionType })
                });
        });
    });
});
