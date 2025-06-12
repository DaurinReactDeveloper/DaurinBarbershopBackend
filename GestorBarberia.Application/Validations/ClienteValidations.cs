using GestorBarberia.Application.Dtos.ClienteDto;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class ClienteValidations
    {

        public static bool ValidationsCountCliente(List<ClienteModel> clientes) {

            if (clientes.Count() <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool ValidationAddCliente(ClienteAddDto clienteAdd)
        {

            if (string.IsNullOrWhiteSpace(clienteAdd.Nombre) || string.IsNullOrEmpty(clienteAdd.Nombre)
           || string.IsNullOrWhiteSpace(clienteAdd.Telefono) || string.IsNullOrEmpty(clienteAdd.Telefono)
           || string.IsNullOrWhiteSpace(clienteAdd.Email) || string.IsNullOrEmpty(clienteAdd.Email)
           || string.IsNullOrWhiteSpace(clienteAdd.Password) || string.IsNullOrEmpty(clienteAdd.Password)
           || string.IsNullOrWhiteSpace(clienteAdd.Imgcliente) || string.IsNullOrEmpty(clienteAdd.Imgcliente)
           )

            {

                return false;

            }

            if (clienteAdd.Nombre.Length >= 29 || clienteAdd.Nombre.Length <= 5
             || clienteAdd.Telefono.Length >= 20 || clienteAdd.Telefono.Length <= 5
             || clienteAdd.Email.Length >= 49 || clienteAdd.Email.Length <= 5
             || clienteAdd.Password.Length >= 99 || clienteAdd.Password.Length <= 5
             || clienteAdd.Imgcliente.Length >= 254 || clienteAdd.Imgcliente.Length <= 5)
            {

                return false;

            }


            return true;

        }

        public static bool ValidationUpdateCliente(ClienteUpdateDto clienteAdd)
        {

            if (string.IsNullOrWhiteSpace(clienteAdd.Nombre) || string.IsNullOrEmpty(clienteAdd.Nombre)
           || string.IsNullOrWhiteSpace(clienteAdd.Telefono) || string.IsNullOrEmpty(clienteAdd.Telefono)
           || string.IsNullOrWhiteSpace(clienteAdd.Email) || string.IsNullOrEmpty(clienteAdd.Email)
           || string.IsNullOrWhiteSpace(clienteAdd.Password) || string.IsNullOrEmpty(clienteAdd.Password)
           || string.IsNullOrWhiteSpace(clienteAdd.Imgcliente) || string.IsNullOrEmpty(clienteAdd.Imgcliente)
           )

            {

                return false;

            }

            if (clienteAdd.Nombre.Length >= 29 || clienteAdd.Nombre.Length <= 5
             || clienteAdd.Telefono.Length >= 20 || clienteAdd.Telefono.Length <= 5
             || clienteAdd.Email.Length >= 49 || clienteAdd.Email.Length <= 5
             || clienteAdd.Password.Length >= 99 || clienteAdd.Password.Length <= 5
             || clienteAdd.Imgcliente.Length >= 254 || clienteAdd.Imgcliente.Length <= 5)
            {

                return false;

            }


            return true;

        }

        public static bool ValidationRemoveCliente(Clientes clienteId)
        {

            if (clienteId.ClienteId <= 0)
            {

                return false;

            }

            return true;

        }

        public static bool ValidationIdCliente(Clientes clienteId)
        {

            if (clienteId is null)
            {

                return false;

            }

            return true;

        }

        public static bool ValidationNameCliente(ClienteModel clienteModel)
        {

            if (clienteModel is null)
            {

                return false;

            }

            return true;

        }

    }
}
