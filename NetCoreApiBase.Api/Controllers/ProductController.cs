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
using System.Linq;
using System.Threading.Tasks;

namespace netcore3_api_basicproject.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {

        private readonly RepositoryContext _context;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public ProductController(RepositoryContext context,
                                 IRepositoryWrapper repositoryWrapper,
                                 IMapper mapper)
        {
            this._context = context;
            this._repoWrapper = repositoryWrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            try
            {
                //var products = await context.
                //                    Products.
                //                    Include(x => x.Category).
                //                    AsNoTracking().
                //                    ToListAsync();

                var products = this._repoWrapper.
                                    Product.
                                    FindAll().
                                    Include(x => x.Category).
                                    AsNoTracking().
                                    ToList();

                var productsDtoList = this._mapper.Map<IEnumerable<ProductDto>>(products);

                return Ok(productsDtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            try
            {
                var product = await _context.
                                   Products.
                                   Include(x => x.Category).
                                   AsNoTracking().
                                   FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                    return NotFound(new { message = "Produto não encontrado!" });


                return Ok(this._mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet]//products/categories/1 -----> trará todos os produtos da categoria 1
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> GetByCategory(int id)
        {
            try
            {
                var products = await _context.
                                    Products.
                                    Include(x => x.Category).
                                    AsNoTracking().
                                    Where(x => x.CategoryId == id).
                                    ToListAsync();

                return Ok(this._mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        //[AllowAnonymous]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ProductDto>> Post([FromBody] ProductDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest("Produto não foi informado!");

            try
            {
                var productDomain = this._mapper.Map<Product>(model);

                _context.Products.Add(productDomain);
                await _context.SaveChangesAsync();

                return Ok(this._mapper.Map<ProductDto>(productDomain));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ProductDto>> Put(int id, [FromBody] ProductDto model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest("Produto não foi informado");

            if (id != model.Id)
                return NotFound(new { message = "Produto não encontrado" });

            try
            {
                var productDomain = this._mapper.Map<Product>(model);

                _context.Entry<Product>(productDomain).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(this._mapper.Map<ProductDto>(productDomain));
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este produto já foi atualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ProductDto>> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok(this._mapper.Map<ProductDto>(product));
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este produto já foi removido" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
