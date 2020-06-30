using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetCoreApiBase.Api;
using NetCoreApiBase.Api.Services;
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
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public UserController(RepositoryContext context,
                              IOptions<AppSettings> appSettings,
                              IRepositoryWrapper repoWrapper,
                              IMapper mapper)
        {
            this._context = context;
            this._appSettings = appSettings.Value;
            this._repoWrapper = repoWrapper;
            this._mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model == null)
                return BadRequest("Usuário não foi informado!");

            model.Role = "Employee";

            try
            {
                var userDomain = this._mapper.Map<User>(model);
                _context.Users.Add(userDomain);
                await _context.SaveChangesAsync();

                userDomain.Password = "";

                return Ok(this._mapper.Map<UserDto>(userDomain));

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserDto model)
        {
            if (model == null)
                return BadRequest("Usuário não foi informado!");

            try
            {
                var user = await _context.
                               Users.
                               AsNoTracking().
                               Where(x => x.Username.ToUpper() == model.Username.ToUpper() &&
                                  x.Password.ToUpper() == model.Password).
                               FirstOrDefaultAsync();

                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos!" });

                var token = TokenService.GenerateToken(this._mapper.Map<User>(model), this._appSettings);

                user.Password = "";
                return Ok(new { user = this._mapper.Map<UserDto>(user), token = token });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            try
            {
                var users = await _context.Users.AsNoTracking().ToListAsync();
                return Ok(this._mapper.Map<IEnumerable<UserDto>>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // METHODS ONLY FOR TESTS:
        //[HttpGet]
        //[Route("Anonimo")]
        //[AllowAnonymous]
        //public string Anonimo() => "anônimo";

        //[HttpGet]
        //[Route("Authenticado")]
        //[Authorize]
        //public string Authenticado() => "authenticado";

        //[HttpGet]
        //[Route("Funcionario")]
        //[Authorize(Roles ="Employee")]
        //public string Funcionario() => "funcionario";

        //[HttpGet]
        //[Route("Gerente")]
        //[Authorize(Roles = "Manager")]
        //public string Gerente() => "gerente";
    }
}
