using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Dtos.AdminDto;
using GestorBarberia.Application.Dtos.BarberoDto;
using GestorBarberia.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {

        private readonly IAdministradorServices administradorServices;
        private readonly IConfiguration configuration;

        public AdministradorController(IAdministradorServices administradorServices, IConfiguration configuration)
        {
            this.administradorServices = administradorServices;
            this.configuration = configuration;
        }

        // GET: api/<AdministradorController>
        [HttpGet("GetAdministradores")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult GetAdministradores()
        {
            var result = this.administradorServices.GetAllAdministradores();

            if (result.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET: api/<AdministradorController>
        [HttpGet("GetAdministradorBarberia/{name}/{password}")]
        public IActionResult GetAdministradorBarberia(string name, string password)
        {
            var result = this.administradorServices.GetAdministradorPropietarioBarberia(name, password);

            if (result.Success == false)
            {
                return BadRequest(result);
            }

            var token = TokenServices.GenerateToken(name, "admin", configuration);

            return Ok(new
            {
                Data = result,
                Token = token
            });
        }

        // GET: api/<AdministradorController>
        [HttpGet("GetAdministradorApp/{name}/{password}")]
        public IActionResult GetAdministradorApp(string name, string password)
        {
            var result = this.administradorServices.GetAdministradorPropietarioApp(name, password);

            if (result.Success == false)
            {
                return BadRequest(result);
            }

            var token = TokenServices.GenerateToken(name, "adminDaurin", configuration);

            return Ok(new
            {
                Data = result,
                Token = token
            });
        }

        // GET: api/<AdministradorController>
        [HttpGet("GetAdministradorById/{id}")]
        public IActionResult GetAdministradorById(int id)
        {
            var result = this.administradorServices.GetAdministradorById(id);

            if (result.Success == false)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }

        // POST api/<AdministradorController>
        [HttpPost("Save")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Post([FromBody] AdminAddDto modelDto)
        {

            var result = this.administradorServices.Add(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<AdministradorController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Put([FromBody] AdminUpdateDto modelDto)
        {

            var result = this.administradorServices.Update(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // DELETE api/<AdministradorController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Delete([FromBody] AdminRemoveDto modelDto)
        {

            var result = this.administradorServices.Remove(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

    }

}
