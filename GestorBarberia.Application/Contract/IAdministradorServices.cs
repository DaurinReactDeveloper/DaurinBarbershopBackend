using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.AdminDto;
using GestorBarberia.Application.Dtos.CitaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Contract
{
    public interface IAdministradorServices : IBaseServices<AdminAddDto, AdminRemoveDto, AdminUpdateDto>
    {

        ServiceResult GetAdministradorPropietarioBarberia(string nombre, string password);
        ServiceResult GetAdministradorPropietarioApp(string nombre, string password);
        ServiceResult GetAllAdministradores();
        ServiceResult GetAdministradorById(int id);

    }
}
