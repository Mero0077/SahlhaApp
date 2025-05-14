using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;


namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckOutController : ControllerBase
    {

        //public void UpdateCartWithProviderAndTaskDetails(Order order)
        //{
            
        //    // After updating, we can proceed to create the Stripe session for checkout.
        //    var stripeOptions = CreateStripeOptions(order);
        //    AddStripeLines(order.CartItems, stripeOptions);

        //    // Send the session to Stripe (this part would typically go to a Stripe API call)
        //    var sessionService = new Stripe.Checkout.SessionService();
        //    var session = sessionService.Create(stripeOptions);

        //    // Send back the session ID to the frontend to redirect the user to the Stripe checkout page
        //    return session.Id;
        //}


        //public Stripe.Checkout.SessionCreateOptions CreateStripeOptions(Order order)
        //{
        //    var options = new Stripe.Checkout.SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.Id}",
        //        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
        //    };
        //    return options;
        //}

        //// add stripe lines
        //// Add the selected cart items to the Stripe checkout session
        //public void AddStripeLines( Dispute cart, Stripe.Checkout.SessionCreateOptions options)
        //{
            

        //        // Add the line item to Stripe
        //        options.LineItems.Add(new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                Currency = "egp", // Use the correct currency (EGP in this case)
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = cart.Product.Title, // Name of the product
        //                    Description = cart.Product.Description, // Description of the product
        //                },
        //                UnitAmount = (long)(cart.Product.Price * 100), // Convert price to the smallest unit (cents)
        //            },
                  
        //        });
            
        //}

    }
}
