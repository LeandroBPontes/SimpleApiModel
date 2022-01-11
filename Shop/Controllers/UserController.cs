using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers {
    [Route("users")]
    public class UserController : ControllerBase {
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context) {

            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> GetById(int id, [FromServices] DataContext context) {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(user);

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] User model,
            [FromServices] DataContext context) {

            var user = await context.Users.AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();
            

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválido" });

            var token = TokenService.GenerateToken(user);

            return new {
                user = user,
                token = token
            };
        }


        [HttpPost]
        public async Task<ActionResult<User>> Post(
            [FromBody] User model,
            [FromServices] DataContext context) {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Users.Add(model);
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
