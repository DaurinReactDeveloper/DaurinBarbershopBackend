using GestorBarberia.Application.Dtos.BarberoDto;
using GestorBarberia.Application.Dtos.CitaDto;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class CitaValidations
    {

        public static bool ValidationsCountCita(List<CitaModel> citas)
        {

            if (citas is null) {

                return false;
            }

            return true;
        }

        public static bool ValidationsCitaId(CitaModel cita)
        {

            if (cita is null)
            {

                return false;
            }

            return true;
        }


        public static bool ValidationsCita(CitaDto citaDto)
        {
            // Verificar si la fecha es nula
            if (citaDto.Fecha == null)
            {
                return false;
            }

            // Verificar si la fecha es anterior a la fecha actual
            if (citaDto.Fecha < DateTime.Now.Date)
            {
                return false;
            }

            // Verificar la hora
            var horaSeleccionada = citaDto.Hora;

            // Definir rango permitido de horas
            var horaMinima = new TimeSpan(9, 0, 0); // 09:00
            var horaMaxima = new TimeSpan(18, 0, 0); // 18:00

            // Verificar que la hora esté en el rango permitido
            if (horaSeleccionada < horaMinima || horaSeleccionada > horaMaxima)
            {
                return false;
            }

            // Verificar si CitaId, ClienteId o EstiloId son nulos
            if (citaDto.CitaId <= 0 || citaDto.ClienteId <= 0 || citaDto.EstiloId <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool ValidationAddCita(CitaAddDto citaDto)
        {

            // Verificar si la fecha es nula o anterior a la fecha actual
            if (citaDto.Fecha == null || citaDto.Fecha.Date < DateTime.Now.Date)
            {
                return false;
            }

            // Verificar la hora
            var horaSeleccionada = citaDto.Hora;

            // Definir rango permitido de horas
            var horaMinima = new TimeSpan(9, 0, 0); // 09:00
            var horaMaxima = new TimeSpan(18, 0, 0); // 18:00

            // Verificar que la hora esté en el rango permitido
            if (horaSeleccionada < horaMinima || horaSeleccionada > horaMaxima)
            { 
                return false;
            }

            // Verificar si ClienteId o EstiloId son nulos
            if (citaDto.ClienteId <= 0 || citaDto.EstiloId <= 0)
            {
                return false;
            }

            return true;
        
        }

        public static bool ValidationUpdateCita(CitaUpdateDto citaDto)
        {

            // Verificar si la fecha es nula o anterior a la fecha actual
            if (citaDto.Fecha == null || citaDto.Fecha.Date < DateTime.Now.Date)
            {
                return false;
            }

            // Verificar la hora
            var horaSeleccionada = citaDto.Hora;

            // Definir rango permitido de horas
            var horaMinima = new TimeSpan(9, 0, 0); // 09:00
            var horaMaxima = new TimeSpan(18, 0, 0); // 18:00

            // Verificar que la hora esté en el rango permitido
            if (horaSeleccionada < horaMinima || horaSeleccionada > horaMaxima)
            {
                return false;
            }

            // Verificar si CitaId, ClienteId o EstiloId son nulos
            if (citaDto.CitaId <= 0)
            {
                return false;
            }

            return true;

        }

        public static bool ValidationRemoveCita(CitaRemoveDto citaDto)
        {

            if (citaDto.Estado != "Rechazada" && citaDto.Estado != "Realizada" && citaDto.Estado != "Cancelada")
            {
                return false;
            }

            return true;
        }

        public static bool ValidationIdCita(Citas citaId)
        {

            if (citaId.CitaId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
