using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context) {

            var users = await context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> GetById(int id, [FromServices] DataContext context) {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(user);

        }

        [HttpPost]
        //[Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Post(
           [FromBody] User model,
           [FromServices] DataContext context) {


            /*
             * todo:
             *forca o usuario a ser sempre employee
             * model.Role = "employee";
             * 
             * aprender a encriptar essa senha
             */

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Users.Add(model);
                //gera um id automatico e incrementa
                await context.SaveChangesAsync();

                //esconde a senha
                model.Password = "";
                return Ok(model);
            }
            catch (Exception) {
                return BadRequest(ModelState);
            }

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
            //esconde a senha
            model.Password = "";


            return new {
                user = user,
                token = token
            };
        }

        [HttpPut]
        [Authorize(Roles = "manager")]
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
        [Authorize(Roles = "manager")]
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

        /*
         [HttpGet]
        [Route("anonimo")]
        [AllowAnonymous]
        public string Anonimo() => "Anonimo";

        [HttpGet]
        [Route("autenticado")]
        [Authorize]
        public string Autenticado() => "Autenticado";

        [HttpGet]
        [Route("funcionario")]
        [Authorize(Roles="employee")]
        public string Funcionario() => "funcioario";

        [HttpGet]
        [Route("gerente")]
        [Authorize(Roles = "manager")]
        public string Gerente() => "Gerente";
         
         */
    }
}
