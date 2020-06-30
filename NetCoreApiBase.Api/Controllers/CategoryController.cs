using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetCoreApiBase.Contracts;
using NetCoreApiBase.Domain;
using NetCoreApiBase.Domain.DTO;
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
        private readonly RepositoryContext _context;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public CategoryController(RepositoryContext context,
                                  IRepositoryWrapper repoWrapper,
                                  IMapper mapper)
        {
            this._context = context;
            this._repoWrapper = repoWrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        //remove o cache individualmente, caso use o cache no config do startup
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<CategoryDto>>> Get()
        {
            try
            {
                //para trazer dados com paginação do asp.net core:
                //https://code-maze.com/paging-aspnet-core-webapi/
                var categories = _repoWrapper.Category.FindAll();
                //var categories = await context.Categories.AsNoTracking().ToListAsync();

                //exemplos do automapper: https://code-maze.com/automapper-net-core/
                var categoriesResult = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return Ok(categoriesResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            try
            {
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new { message = "Categoria não encontrada" });
                return Ok(this._mapper.Map<CategoryDto>(category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<CategoryDto>> Post([FromBody] CategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest("Categoria não foi informada!");

            try
            {
                var categoryDomain = this._mapper.Map<Category>(model);

                _context.Categories.Add(categoryDomain);
                await _context.SaveChangesAsync();


                return Ok(this._mapper.Map<CategoryDto>(categoryDomain));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<CategoryDto>> Put(int id, [FromBody] CategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest("Categoria não foi informada!");

            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                var categoryDomain = this._mapper.Map<Category>(model);

                _context.Entry<Category>(categoryDomain).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(this._mapper.Map<Category>(categoryDomain));
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esta categoria já foi atualizada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [AllowAnonymous]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<dynamic>> Delete(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            //TODO:
            //fazer a amarração da tabela de categoria e da tabela de produto para não permitir a
            //exclusão física. e depois gerar o banco novamente.


            if (_repoWrapper.Product.ExistsProductsByCategoryId(id))
                return BadRequest("Categoria está atrelada a um produto e não pode ser removida!");

            try
            {

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esta categoria já foi removida" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
