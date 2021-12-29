using Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shop.Controllers {
    [Route("categories")]
    public class CategoryController : ControllerBase {

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context) {

            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context) {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);

        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post(
            [FromBody] Category model,
            [FromServices] DataContext context) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Categories.Add(model);
                //gera um id automatico e incrementa
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception) {
                return BadRequest(ModelState);
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(
            int id,
            [FromBody] Category model,
            [FromServices] DataContext context) {

            if (model.Id != id)
                return NotFound(new { message = "Categoria não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return model;
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest(new { message = "Esse Registro já foi atualizado" });
            }
            catch (Exception) {
                return BadRequest(new { message = "Não foi possível atualizar essa categoria" });
            }

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Delete(
            int id,
            [FromServices] DataContext context) {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new { message = "Categoria removida com sucesso!" });
            }
            catch (Exception) {
                return BadRequest(new { message = "Não foi possível remover a categoria" });
            }
        }
    }
}