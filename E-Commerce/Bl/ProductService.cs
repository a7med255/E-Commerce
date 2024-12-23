using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Bl
{
    public interface IProductService
    {
        IEnumerable<TbItem> GetNewProducts();
        IEnumerable<TbItem> GetManProducts();
        IEnumerable<TbItem> GetWomanProducts();
        TbItem GetProductById(int id);
        IEnumerable<TbItem> GetAllProducts();

    }

    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;

        public ProductService(ECommerceContext context)
        {
            _context = context;
        }
        public TbItem GetProductById(int id)
        {
            return  _context.TbItems.FirstOrDefault(a=>a.ItemId==id);
        }

        public IEnumerable<TbItem> GetAllProducts()
        {
            return _context.TbItems.ToList();
        }

        public IEnumerable<TbItem> GetNewProducts()
        {
            return _context.TbItems.OrderByDescending(a=>a.CreatedDate).Take(9).ToList();
        }

        public IEnumerable<TbItem> GetManProducts()
        {
            return _context.TbItems.Where(p => p.ItemType.ItemTypeName == "Man").ToList();
        }

        public IEnumerable<TbItem> GetWomanProducts()
        {
            return _context.TbItems.Where(p => p.ItemType.ItemTypeName == "Woman").ToList();
        }
    }
}
