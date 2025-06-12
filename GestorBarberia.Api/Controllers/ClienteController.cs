using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Dtos.ClienteDto;
using GestorBarberia.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteServices clienteServices;
        private readonly IConfiguration configuration;

        public ClienteController(IClienteServices clienteServices, IConfiguration configuration)
        {
            this.clienteServices = clienteServices;
            this.configuration = configuration;
        }

        // GET: api/<ClienteController>
        [HttpGet("GetCliente/{name}/{password}")]
        public IActionResult GetCliente(string name, string password)
        {

            var result = this.clienteServices.GetCliente(name, password);
            var token = TokenServices.GenerateToken(name, "cliente", configuration);

            if (result.Success == false)
            {

                return BadRequest(result);

            }

            return Ok(new
            {
                Data = result,
                Token = token
            });

        }

        // GET: api/<ClienteController>
        [HttpGet("GetClientes")]
        [Authorize(Roles = "barbero,adminDaurin")]
        public IActionResult GetClientes()
        {

            var result = this.clienteServices.GetClientes();


            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<ClienteController>/5
        [HttpGet("ClienteById/{id}")]
        [Authorize(Roles = "barbero,cliente,admin,adminDaurin")]
        public IActionResult ClienteById(int id)
        {

            var result = this.clienteServices. GetClienteById(id);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<ClienteController>/5
        [HttpGet("ClienteByBarberiaId/{id}")]
        [Authorize(Roles = "admin,adminDaurin")]
        public IActionResult ClienteByBarberiaId(int id)
        {

            var result = this.clienteServices.GetClientesbyBarberiaId(id);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // POST api/<ClienteController>
        [HttpPost("Save")]
        public IActionResult Post([FromBody] ClienteAddDto modelDto)
        {

            var result = this.clienteServices.Add(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<ClienteController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Put([FromBody] ClienteUpdateDto modelDto)
        {

            var result = this.clienteServices.Update(modelDto);

            if(result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Delete([FromBody] ClienteRemoveDto modelDto)
        {

            var result = this.clienteServices.Remove(modelDto);

            if(result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("DeleteByAdmin/{clienteId}/{adminId}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteByAdmin(int clienteId, int adminId)
        {

            var result = this.clienteServices.RemoveClienteByAdmin(clienteId, adminId);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }
    }
}
