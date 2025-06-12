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
    public static class BarberoValidations
    {

        public static bool ValidationCountBarbero(List<BarberoModel> barberos)
        {

            if (barberos.Count() <= 0)
            {

                return false;

            }

            return true;

        }

        public static bool ValidationAddBarbero(BarberoAddDto barberoDto)
        {

            if (barberoDto.Nombre.Length >= 29 || barberoDto.Nombre.Length <= 5
               || barberoDto.Telefono.Length >= 20 || barberoDto.Telefono.Length <= 5
               || barberoDto.Email.Length >= 49 || barberoDto.Email.Length <= 5
               || barberoDto.Password.Length >= 99 || barberoDto.Password.Length <= 5
               || barberoDto.Imgbarbero.Length >= 254 || barberoDto.Imgbarbero.Length <= 5
               )
            {

                return false;

            }

            return true;

        }

        public static bool ValidationUpdateBarbero(BarberoUpdateDto barberoDto)
        {

            if (barberoDto.Nombre.Length >= 29 || barberoDto.Nombre.Length <= 5
               || barberoDto.Telefono.Length >= 20 || barberoDto.Telefono.Length <= 5
               || barberoDto.Email.Length >= 49 || barberoDto.Email.Length <= 5
               || barberoDto.Password.Length >= 99 || barberoDto.Password.Length <= 5
               || barberoDto.Imgbarbero.Length >= 254 || barberoDto.Imgbarbero.Length <= 5
               )
            {

                return false;

            }

            return true;

        }

        public static bool ValidationRemoveBarbero(Barberos barberoId)
        {

            if (barberoId.BarberoId <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool ValidationIdBarbero(Barberos barberoId)
        {

            if (barberoId is null)
            {
                return false;
            }

            return true;
        }

        public static bool ValidationNameBarberoModel(BarberoModel barberoName)
        {

            if (barberoName is null)
            {
                return false;
            }

            return true;
        }

    }
}
