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

        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public ProductController(IRepositoryWrapper repositoryWrapper,
                                 IMapper mapper)
        {
           _repoWrapper = repositoryWrapper;
            this._mapper = mapper;
        }

        //[HttpGet]
        //[Route("")]
        //[AllowAnonymous]
        //public async Task<ActionResult<String>> Get()
        //{
        //    try
        //    {
        //      return Ok("Teste OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error: " + ex.Message);
        //    }
        //}


        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            try
            {
                var products = await this._repoWrapper.
                                     Product.
                                     FindAll().
                                     Include(x => x.Category).
                                     ToListAsync();

                var productsDtoList = this._mapper.Map<IEnumerable<ProductDto>>(products);

                return Ok(new { total = productsDtoList.Count(), result = productsDtoList });
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
                var product = await _repoWrapper.
                                   Product.
                                   FindAll().
                                   Include(x => x.Category).
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
                var products = await _repoWrapper.
                                    Product.
                                    FindAll().
                                    Include(x => x.Category).
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
        //[Authorize(Roles = "Manager")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Post([FromBody] ProductDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest("Produto não foi informado!");

            try
            {
                var productDomain = this._mapper.Map<Product>(model);

               await _repoWrapper.Product.Create(productDomain);

                //return Ok(this._mapper.Map<ProductDto>(productDomain));
                return Ok(new { result = true, message = "" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        //[Authorize(Roles = "Manager")]
        [AllowAnonymous]

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

                await _repoWrapper.Product.Update(productDomain);

                // return Ok(this._mapper.Map<ProductDto>(productDomain));
                return Ok(new { result = true, message = "Produto Alterado com sucesso!" });
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
        ///[Authorize(Roles = "Manager")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Delete(int id)
        {
            var products = await _repoWrapper.Product.FindByConditionAsync(x => x.Id == id);

            if (products == null || products.Count() <= 0)
                return NotFound(new { message = "Produto não encontrado" });

            try
            {
               await _repoWrapper.Product.Delete(products.FirstOrDefault());

                //return Ok(this._mapper.Map<ProductDto>(products.FirstOrDefault()));
                return Ok(new { result = true, message = "Produto Excluído com sucesso!" });
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


        [AllowAnonymous]
        [HttpGet]
        [Route("GetTeste")]
        public ActionResult<Product> GetTeste()
        {
            int[] scores = { 90, 71, 82, 93, 75, 82 };

            IEnumerable<int> query = from score in scores where score > 80 orderby score descending select score;

            var teste = query;

            foreach (int testScore in query)
            {
                Console.WriteLine(testScore);
            }


            List<City> citiesSource = new List<City>();
            citiesSource.Add(new City() { Id = 1, Population = 10000 });
            citiesSource.Add(new City() { Id = 2, Population = 2344 });
            citiesSource.Add(new City() { Id = 3, Population = 4555555 });
            citiesSource.Add(new City() { Id = 4, Population = 21345 });
            citiesSource.Add(new City() { Id = 5, Population = 245 });
            citiesSource.Add(new City() { Id = 6, Population = 5000 });


            ////ExemploQuery Syntax:
            //IEnumerable<City> majorCities = from city in citiesSource where city.Population > 9000 group city by city.Population into biggestCities
            //                                from smallCities in citiesSource where smallCities.Population < 9000 group smallCities by smallCities.Population into smallersCities
            //                                select


            ////Method base-syntax:
            //IEnumerable<City> majorCities2 = citiesSource.Where(x => x.Population > 9000);





            //IEnumerable<Product> list = _repoWrapper.Product.FindAll().Where(p => p.CategoryId == 1);
            //list = list.Take<Product>(3);

            //var testeIEnume = list.FirstOrDefault();


            IQueryable<Product> queryP = _repoWrapper.Product.FindAll().Where(p => p.CategoryId == 1);
            queryP = queryP.Take<Product>(3);

            List<Product> novLista = new List<Product>();

            foreach (var item in queryP)
            {
                novLista.Add(item);
            }

            return novLista.FirstOrDefault();

        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("GetTesteEmployee")]
        //public ActionResult<dynamic> GetTesteEmployee()
        //{
        //    Nort
        //}
    }

    public class City
    {
        public int Id { get; set; }
        public int Population { get; set; }

    }

}

