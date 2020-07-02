using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCoreApiBase.Api;
using NetCoreApiBase.Api.Services;
using NetCoreApiBase.Contracts;
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
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public UserController(IOptions<AppSettings> appSettings,
                              IRepositoryWrapper repoWrapper,
                              IMapper mapper)
        {
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
                await _repoWrapper.User.Create(userDomain);

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
                var users = await _repoWrapper.
                               User.
                               FindByConditionAsync(x => x.Username.ToUpper() == model.Username.ToUpper() &&
                                  x.Password.ToUpper() == model.Password);

                if (users == null || users.Count() <= 0)
                    return NotFound(new { message = "Usuário ou senha inválidos!" });

                var token = TokenService.GenerateToken(this._mapper.Map<User>(users.FirstOrDefault()), this._appSettings);

                var user = users.FirstOrDefault();

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
                var users = await _repoWrapper.User.FindAllAsync();
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
