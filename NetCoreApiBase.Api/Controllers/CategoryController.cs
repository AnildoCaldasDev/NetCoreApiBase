using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using NetCoreApiBase.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace netcore3_api_basicproject.Controllers
{

    //endpoints:
    //http://localhost:5000
    //htpps://localhost:50001



    //[Route("api/[controller]")]
    //[ApiController]

    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {

        private readonly RepositoryContext context;

        private IRepositoryWrapper _repoWrapper;

        public CategoryController(RepositoryContext _context, IRepositoryWrapper repoWrapper)
        {
            this.context = _context;
            this._repoWrapper = repoWrapper;
        }


        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30 )]
        //remove o cache individualmente, caso use o cache no config do startup
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get()
        {
            var categories = _repoWrapper.Category.FindAll();

            //var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }


        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> Get(int id) {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            return Ok(category);
        }


        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Category>> Post([FromBody] Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel inserir a categoria" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esta categoria já foi atualizada" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar a categoria" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Category>> Delete(int id)
        {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {

                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso!"});
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esta categoria já foi removida" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria" });
            }
        }
    }
}
