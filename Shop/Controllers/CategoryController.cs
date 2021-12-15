using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("categories")]
public class CategoryController : ControllerBase{
    
    [HttpGet]
    public async Task<ActionResult<List<Category>>> Get()
    {
        return new List<Category>();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        return new Category();
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Post([FromBody]Category model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(model);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model)
    {
       
        if (model.Id != id)
            return NotFound(new { message = "Categoria não encontrada" });
       
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(model);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Delete(int id)
    {
        return Ok();
    }

}