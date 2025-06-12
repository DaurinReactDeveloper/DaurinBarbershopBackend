using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Dtos.ClienteDto;
using GestorBarberia.Application.Dtos.EstilodecorteDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilosdecorteController : ControllerBase
    {

        private readonly IEstilosdecorteServices estiloServices;

        public EstilosdecorteController(IEstilosdecorteServices estiloServices)
        {
            this.estiloServices = estiloServices;
        }

        // GET: api/<EstilodecorteController>
        [HttpGet("GetEstilos")]
        [Authorize(Roles = "barbero,cliente,admin,adminDaurin")]
        public IActionResult Get()
        {

            var result = this.estiloServices.GetEstilos();


            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<EstilodecorteController>
        [HttpGet("Estilos/{id}")]
        [Authorize(Roles = "admin,cliente")]
        public IActionResult Get(int id)
        {

            var result = this.estiloServices.GetEstilosById(id);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<EstilodecorteController>/5
        [HttpGet("EstilosByBarberiaId/{id}")]
        [Authorize(Roles = "cliente,barbero,admin,adminDaurin")]
        public IActionResult EstilosByBarberiaId(int id)
        {

            var result = this.estiloServices.GetEstilosbyBarberiaId(id);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // GET api/<EstilodecorteController>/5
        [HttpGet("EstilosByClienteBarberiaId/{id}")]
        [Authorize(Roles = "cliente,barbero")]
        public IActionResult EstilosByClienteBarberiaId(int id)
        {

            var result = this.estiloServices.GetEstilosbyCliente(id);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);


        }

        // POST api/<EstilodecorteController>
        [HttpPost("Save")]
        [Authorize(Roles = "admin,adminDaurin")]
        public IActionResult Post([FromBody] EstilosdecorteAddDto modelDto)
        {

            var result = this.estiloServices.Add(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<EstilodecorteController>
        [HttpPut("Update")]
        [Authorize(Roles = "admin,adminDaurin")]
        public IActionResult Put([FromBody] EstilosdecorteUpdateDto modelDto)
        {

            var result = this.estiloServices.Update(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // DELETE api/<EstilodecorteController>
        [HttpDelete("Delete")]
        [Authorize(Roles = "admin,adminDaurin")]
        public IActionResult Remove(EstilosdecorteRemoveDto modelDto)
        {

            var result = this.estiloServices.Remove(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // DELETE api/<EstilodecorteController>
        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteByAdmin/{estilioId}/{adminId}")]
        public IActionResult DeleteByAdmin(int estilioId, int adminId)
        {

            var result = this.estiloServices.RemoveEstilosByAdmin(estilioId, adminId);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }
    }
}
