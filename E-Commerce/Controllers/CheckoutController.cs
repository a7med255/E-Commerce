using E_Commerce.Bl;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ECommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(ECommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            // Get the current user from the token
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Fetch cart items for the user
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }

            // Calculate total amount
            var totalAmount = cartItems.Sum(c => c.Quantity * c.Item.SalesPrice);

            // Create a new order
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Item.SalesPrice,
                }).ToList()
            };

            _context.Orders.Add(order);

            // Clear cart items
            _context.CartItems.RemoveRange(cartItems);

            // Save changes
            await _context.SaveChangesAsync();

            // Return order details
            return Ok(new
            {
                Message = "Checkout successful",
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate
            });
        }
        


    }
}
