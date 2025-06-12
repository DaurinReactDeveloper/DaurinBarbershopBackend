using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Dtos.BarberiaDto;
using GestorBarberia.Application.Dtos.CitaDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarberiaController : ControllerBase
    {
        private readonly IBarberiaServices barberiaServices;

        public BarberiaController(IBarberiaServices barberiaServices)
        {
            this.barberiaServices = barberiaServices;   
        }

        // GET: api/<BarberiaController>
        [HttpGet("GetBarberias")]
        public IActionResult GetBarberias()
        {

            var result = this.barberiaServices.GetBarberias();

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        //POST api/<BarberiaController>
        [HttpPost("Save")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Post([FromBody] BarberiaAddDto modelDto)
        {

            var result = this.barberiaServices.Add(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<BarberiaController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Put([FromBody] BarberiaUpdateDto modelDto)
        {

            var result = this.barberiaServices.Update(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // DELETE api/<BarberiaController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "adminDaurin")]
        public IActionResult Delete([FromBody] BarberiaRemoveDto modelDto)
        {

            var result = this.barberiaServices.Remove(modelDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

    }
}
