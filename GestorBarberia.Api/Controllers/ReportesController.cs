using GestorBarberia.Application.Contract;
using GestorBarberia.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorBarberia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {

        private readonly IReporteriaServices reporteriaServices;

        public ReportesController(IReporteriaServices reporteriaServices)
        {
            this.reporteriaServices = reporteriaServices;
        }

        // GET api/<ReportesController>/5
        [HttpGet("GetReportes/{barberiaId}/{fechaInicio}/{fechaFin}")]
        [Authorize(Roles = "admin")]
        public ActionResult GetReportes(int barberiaId, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = this.reporteriaServices.GenerarTablaIngresos(barberiaId, fechaInicio, fechaFin);


            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<ReportesController>/5
        [HttpGet("GetIngresos/{barberiaId}")]
        [Authorize(Roles = "admin")]
        public ActionResult GetIngresos(int barberiaId)
        {
            var result = this.reporteriaServices.ObtenerTotalIngresos(barberiaId);


            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<ReportesController>/5
        [HttpGet("GetTotalClientes/{barberiaId}")]
        [Authorize(Roles = "admin")]
        public ActionResult GetTotalClientes(int barberiaId)
        {
            var result = this.reporteriaServices.ObtenerTotalClientes(barberiaId);


            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // GET api/<ReportesController>/5
        [HttpGet("GetTotalBarberos/{barberiaId}")]
        [Authorize(Roles = "admin")]
        public ActionResult GetTotalBarberos(int barberiaId)
        {
            var result = this.reporteriaServices.ObtenerTotalBarberos(barberiaId);


            if (result is null)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

    }
}
