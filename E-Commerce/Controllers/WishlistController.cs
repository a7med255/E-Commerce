using E_Commerce.Bl;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly ECommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ECommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get wishlist for the current user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbWishlistItem>>> GetWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            var wishlist = await _context.TbWishlistItems.Where(w => w.UserId == userId).ToListAsync();

            return wishlist;
        }

        // Add a product to the wishlist
        [HttpPost]
        public async Task<ActionResult<TbWishlistItem>> AddToWishlist([FromBody] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            var product = await _context.TbItems.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var wishlistItem = new TbWishlistItem
            {
                ProductId = productId,
                UserId = userId,
                DateAdded = DateTime.Now
            };

            _context.TbWishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWishlist), new { id = wishlistItem.Id }, wishlistItem);
        }

        // Remove a product from the wishlist
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var wishlistItem = await _context.TbWishlistItems.FindAsync(id);

            if (wishlistItem == null)
            {
                return NotFound();
            }

            _context.TbWishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
