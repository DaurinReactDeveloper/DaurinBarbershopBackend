using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Dtos.ComentarioDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService comentarioService;

        public ComentarioController(IComentarioService comentarioService)
        {
            this.comentarioService = comentarioService;
        }

        // GET: api/<ComentarioController>
        [HttpGet("GetComentarios")]
        public IActionResult GetComentarios()
        {

            var result = this.comentarioService.GetComentarios();

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET: api/<ComentarioController>
        [HttpGet("GetComentariosBarbero/{BarberoId}")]
        [Authorize(Roles = "cliente,barbero,admin,adminDaurin")]
        public IActionResult GetComentariosBarbero(int BarberoId)
        {

            var result = this.comentarioService.GetComentsByBarbero(BarberoId);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }
        
        // GET: api/<ComentarioController>
        [HttpGet("GetComentariosCliente/{ClienteId}")]
        [Authorize(Roles = "cliente,barbero,admin,adminDaurin")]
        public IActionResult GetComentariosCliente(int ClienteId)
        {

            var result = this.comentarioService.GetComentsByCliente(ClienteId);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // POST api/<ComentarioController>
        [HttpPost("Save")]
        [Authorize(Roles = "cliente")]
        public IActionResult Post([FromBody] ComentarioaddDto comentarioaddDto)
        {

            var result = this.comentarioService.Add(comentarioaddDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<ComentarioController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "cliente")]
        public IActionResult Put([FromBody] ComentarioUpdateDto comentarioUpdateDto)
        {

            var result = this.comentarioService.Update(comentarioUpdateDto);

            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // DELETE api/<ComentarioController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "cliente")]
        public IActionResult Delete([FromBody] ComentarioRemoveDto comentarioRemoveDto)
        {

            var result = this.comentarioService.Remove(comentarioRemoveDto);

            if (result is null)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }
    }
}
