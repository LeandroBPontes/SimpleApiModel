using Shop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Controllers {
    [Route("products")]
    
    public class ProductController : ControllerBase {

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context) {

            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context) {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(products);

        }

        [HttpGet] // products/categories/1
        [Route("categories/{id:int}")]
        public async Task<ActionResult<Product>> GetByCategory(int id, [FromServices] DataContext context) {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();
            return Ok(products);

        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(
           [FromBody] Product model,
           [FromServices] DataContext context) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Products.Add(model);
                //gera um id automatico e incrementa
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception) {
                return BadRequest(ModelState);
            }

        }

    }
}
