using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    // https://localhost:port/api/products
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [Authorize]
        [HttpPost("bulk")] 
        public async Task<ActionResult<IEnumerable<Product>>> PostMultipleProducts([FromBody] List<Product> products)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("The product list cannot be empty.");
            }
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            return Ok(products);
        }
    }
}