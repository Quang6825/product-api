using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tesstt.Data;
using tesstt.DTOs;
using tesstt.Models;

namespace tesstt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await _db.Products
                .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var p = await _db.Products
                .Where(x => x.Id == id)
                .Select(x => new ProductDto { Id = x.Id, Name = x.Name, Price = x.Price })
                .FirstOrDefaultAsync();

            if (p == null) return NotFound();
            return p;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto input)
        {
            var product = new Product { Name = input.Name, Price = input.Price };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var output = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };
            return CreatedAtAction(nameof(GetById), new { id = output.Id }, output);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto input)
        {
            if (id != input.Id) return BadRequest();

            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();

            p.Name = input.Name;
            p.Price = input.Price;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();

            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
