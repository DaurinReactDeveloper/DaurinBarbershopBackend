using GestorBarberia.Application.Dtos.AdminDto;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class AdministradorValidations
    {

        public static bool AdministradorCountValidations(List<AdministradorModel> administradorModels)
        {

            if (administradorModels.Count() <= 0)
            {

                return false;

            }

            return true;
        }

        public static bool AdministradorNameValidations(AdministradorModel model)
        {

            if (model is null)
            {

                return false;

            }

            return true;
        }

        public static bool AdministradorPropietarioBarberiaValidations(string tipo)
        {

            if (tipo != "PropietarioBarberia")
            {

                return false;

            }

            return true;
        }

        public static bool AdministradorPropietarioAppValidations(string tipo)
        {

            if (tipo != "PropietarioApp")
            {

                return false;

            }

            return true;
        }

        public static bool AdministradorIdValidations(AdministradorModel model)
        {

            if (model.AdministradoresId <= 0)
            {

                return false;

            }

            return true;
        }

        public static bool AdministradorAddValidations(AdminAddDto adminAdd)
        {

            if (adminAdd.Nombre.Length >= 29 || adminAdd.Nombre.Length <= 5 
                || adminAdd.Telefono.Length >= 20 || adminAdd.Telefono.Length <= 5 
                || adminAdd.Email.Length >= 49 || adminAdd.Email.Length <= 5
                || adminAdd.Password.Length >= 99 || adminAdd.Password.Length <= 5 
                )
            {
                return false;
            }


            return true;
        }

        public static bool AdministradorUpdateValidations(AdminUpdateDto adminUpdate) {

            if (adminUpdate.Nombre.Length >= 29 || adminUpdate.Nombre.Length <= 5
             || adminUpdate.Telefono.Length >= 20 || adminUpdate.Telefono.Length <= 5
             || adminUpdate.Email.Length >= 49 || adminUpdate.Email.Length <= 5
             || adminUpdate.Password.Length >= 99 || adminUpdate.Password.Length <= 5
             )
            {

                return false;

            }

            return true;

        }

        public static bool AdministradorRemoveValidations(Administradores administrador)
        {

            if (administrador is null)
            {
                return false;
            }

            return true;
        }

        public static bool AdministradorRemoveTipoValidations(Administradores administrador)
        {

            if (administrador.Tipo == "PropietarioApp")
            {
                return false;
            }

            return true;
        }


    }
}
