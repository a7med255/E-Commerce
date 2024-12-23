using Microsoft.AspNetCore.Mvc;
using E_Commerce.Bl;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public HomeController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        // Endpoint لعرض المنتجات الجديدة
        [HttpGet("new-products")]
        public IActionResult GetNewProducts()
        {
            var products = _productService.GetNewProducts();
            if (products == null || !products.Any())
                return NotFound("No new products found.");

            return Ok(products);
        }

        // Endpoint لعرض منتجات الرجال
        [HttpGet("man-products")]
        public IActionResult GetManProducts()
        {
            var products = _productService.GetManProducts();
            if (products == null || !products.Any())
                return NotFound("No man products found.");

            return Ok(products);
        }

        // Endpoint لعرض منتجات النساء
        [HttpGet("woman-products")]
        public IActionResult GetWomanProducts()
        {
            var products = _productService.GetWomanProducts();
            if (products == null || !products.Any())
                return NotFound("No woman products found.");

            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        // Endpoint للتحقق من حالة تسجيل الدخول
        [HttpGet("login-status")]
        public IActionResult GetLoginStatus()
        {
            var user = _userService.GetCurrentUserAsync();
            if (user == null)
                return Ok(new { loggedIn = false });

            return Ok(new { loggedIn = true});
        }
    }
}
