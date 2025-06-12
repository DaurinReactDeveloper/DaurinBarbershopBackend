using GestorBarberia.Application.Dtos.EstilodecorteDto;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class EstilosdecorteValidations
    {

        public static bool ValidationsCountCortes(List<EstilosdecorteModel> estilos)
        {

            if (estilos.Count() <= 0)
            {
                return  false;  
            }

            return true;
        }

        public static bool ValidationAddCortes(EstilosdecorteAddDto estilosdecorteAdd)
        {


            if (string.IsNullOrEmpty(estilosdecorteAdd.Nombre) || string.IsNullOrWhiteSpace(estilosdecorteAdd.Nombre)
           || string.IsNullOrEmpty(estilosdecorteAdd.Descripcion) || string.IsNullOrWhiteSpace(estilosdecorteAdd.Descripcion)
           || string.IsNullOrEmpty(estilosdecorteAdd.Imgestilo) || string.IsNullOrWhiteSpace(estilosdecorteAdd.Imgestilo))

            {

                return false;

            }


            if (estilosdecorteAdd.Nombre.Length >= 30 || estilosdecorteAdd.Nombre.Length <= 5
             || estilosdecorteAdd.Imgestilo.Length >= 255 || estilosdecorteAdd.Imgestilo.Length <= 5 
             || estilosdecorteAdd.Precio >= 1000
             || estilosdecorteAdd.Descripcion.Length >= 54 || estilosdecorteAdd.Descripcion.Length <= 45)
            {

                return false;

            }


            if (estilosdecorteAdd.Precio <= 0)
            {

                return false;

            }


            return true;

        }

        public static bool ValidationUpdateCortes(EstilosdecorteUpdateDto estilosdecorteUpdate)
        {


            if (string.IsNullOrEmpty(estilosdecorteUpdate.Nombre) || string.IsNullOrWhiteSpace(estilosdecorteUpdate.Nombre)
           || string.IsNullOrEmpty(estilosdecorteUpdate.Descripcion) || string.IsNullOrWhiteSpace(estilosdecorteUpdate.Descripcion)
           || string.IsNullOrEmpty(estilosdecorteUpdate.Imgestilo) || string.IsNullOrWhiteSpace(estilosdecorteUpdate.Imgestilo))

            {

                return false;

            }


            if (estilosdecorteUpdate.Nombre.Length >= 30 || estilosdecorteUpdate.Nombre.Length <= 5
             || estilosdecorteUpdate.Imgestilo.Length >= 255 || estilosdecorteUpdate.Imgestilo.Length <= 5 || estilosdecorteUpdate.Precio >= 1000
             || estilosdecorteUpdate.Descripcion.Length >= 54 || estilosdecorteUpdate.Descripcion.Length <= 5)
            {

                return false;

            }


            if (estilosdecorteUpdate.EstiloId <= 0 || estilosdecorteUpdate.Precio <= 0)
            {

                return false;

            }


            return true;

        }

        public static bool ValidationRemoveCortes(Estilosdecortes estilosdecorte)
        {

            if (estilosdecorte.EstiloId <= 0)
            {

                return false;

            }

            return true;

        }

        public static bool ValidationidCortes(Estilosdecortes estilosdecorte)
        {

            if (estilosdecorte.EstiloId <= 0)
            {

                return false;

            }

            return true;

        }

    }
}
