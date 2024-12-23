using E_Commerce.Bl;
using E_Commerce.Models;
using E_Commerce.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ECommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ECommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get the cart for the current user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            var cart = await _context.CartItems.Where(c => c.UserId == userId).Include(c=>c.Item).ToListAsync();

            return cart;
        }

        // Add a product to the cart
        [HttpPost]
        public async Task<ActionResult<CartItem>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            var product = await _context.TbItems.FindAsync(addToCartDto.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            var cartItem = new CartItem
            {
                ProductId = addToCartDto.ProductId,
                UserId = userId,
                Quantity = addToCartDto.Quantity,
                Item = product
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { id = cartItem.Id }, cartItem);
        }

        // Remove a product from the cart
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }



}
