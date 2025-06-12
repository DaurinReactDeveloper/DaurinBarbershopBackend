using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Infrastructure.Extension
{
    public static class ClienteExtension
    {
        public static ClienteModel ConvertClienteToModel(this Clientes ClienteEntity) 
        {

            var clienteModel = new ClienteModel()
            {

                ClienteId = ClienteEntity.ClienteId,
                Email = ClienteEntity.Email,
                Nombre = ClienteEntity.Nombre,
                Telefono = ClienteEntity.Telefono,
                Password = ClienteEntity.Password,
                Imgcliente = ClienteEntity.Imgcliente,
            }; 

            return clienteModel;
        }
    }
}
