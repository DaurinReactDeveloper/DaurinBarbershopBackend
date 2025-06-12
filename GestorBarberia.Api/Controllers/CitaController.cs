using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Dtos.BarberoDto;
using GestorBarberia.Application.Dtos.CitaDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {

        private readonly ICitaServices Citaservices;

        public CitaController(ICitaServices Citaservices)
        {
            this.Citaservices = Citaservices;
        }

        // GET: api/<CitaController>
        [HttpGet("GetCitasById/{clienteId}")]
        public IActionResult GetCitasById(int clienteId)
        {

            var result = this.Citaservices.GetCitaById(clienteId);

            if (result is null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET: api/<CitaController>
        [HttpGet("GetCitasByCliente/{clienteId}")]
        [Authorize(Roles = "barbero,cliente,admin")]
        public IActionResult GetCitasByCliente(int clienteId)
        {

            var result = this.Citaservices.GetCitaByClienteId(clienteId);

            if (result is null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET api/<CitaController>/5
        [HttpGet("GetCitasByBarbero/{barberoId}")]
        [Authorize(Roles = "admin,barbero")]
        public IActionResult GetCitasByBarbero(int barberoId)
        {

            var result = this.Citaservices.GetCitaByBarberoId(barberoId);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        //POST api/<CitaController>
        [HttpPost("Save")]
        [Authorize(Roles = "cliente")]
        public IActionResult Post([FromBody] CitaAddDto modelDto)
        {

            var result = this.Citaservices.Add(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<CitaController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "admin")]
        public IActionResult Put([FromBody] CitaUpdateDto modelDto)
        {

            var result = this.Citaservices.Update(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }
       
        // PUT api/<CitaController>/5
        [HttpPut("UpdateEstado")]
        [Authorize(Roles = "barbero,cliente")]
        public IActionResult PutEstado([FromBody] CitaUpdateDto modelDto)
        {

            var result = this.Citaservices.UpdateEstado(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // DELETE api/<CitaController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "admin,cliente,barbero")]
        public IActionResult Delete([FromBody] CitaRemoveDto modelDto)
        {

            var result = this.Citaservices.Remove(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }
    }
}
