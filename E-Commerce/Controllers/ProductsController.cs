using E_Commerce.Bl;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceContext _context;
        private readonly IProductService _productService;

        public ProductsController(ECommerceContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TbItem>>> GetAllProducts(
            string category = null,
            string type = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string search = null)
        {
            var products = _context.TbItems.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.CategoryName == category);
            }

            if (!string.IsNullOrEmpty(type))
            {
                products = products.Where(p => p.ItemType.ItemTypeName == type);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.SalesPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.SalesPrice <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ItemName.Contains(search));
            }

            return await products.ToListAsync();
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
    }

}
