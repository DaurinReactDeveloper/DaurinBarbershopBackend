using GestorBarberia.Application.Dtos.BarberiaDto;
using GestorBarberia.Application.Dtos.BarberoDto;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class BarberiaValidations
    {

        public static bool ValidationAddBarberia(BarberiaAddDto barberiaDto)
        {

            if (barberiaDto.NombreBarberia.Length >= 44 || barberiaDto.NombreBarberia.Length <= 5)
            {
                return false;
            }

            if (barberiaDto.Admin <= 0)
            {
                return false;
            }

            return true;

        }

        public static bool ValidationCountBarberia(List<BarberiaModel> barberias)
        {

            if (barberias.Count() <= 0)
            {
                return false;
            }


            return true;

        }


        public static bool ValidationUpdateBarberia(BarberiaUpdateDto barberiaDto)
        {

            if (barberiaDto.NombreBarberia.Length >= 44 || barberiaDto.NombreBarberia.Length <= 5)
            {
                return false;
            }

            if (barberiaDto.BarberiasId <= 0)
            {
                return false;
            }


            return true;

        }

        public static bool ValidationRemoveBarberia(Barberias barberiasId)
        {

            if (barberiasId.BarberiasId <= 0)
            {
                return false;
            }

            return true;

        }

        public static bool ValidationBarberiaId(BarberiaModel model)
        {

            if (model.BarberiasId <= 0)
            {

                return false;

            }

            return true;
        }
        public static bool ValidationBarberiaName(BarberiaModel model)
        {

            if (model.NombreBarberia is null)
            {

                return false;

            }

            return true;
        }

    }
}
