using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class ReportesValidations
    {

        public static bool ValidationsBarberiaIdReportes(int barberiaId)
        {

            if (barberiaId < 0)
            {
                return false;
            }

            return true;
        } 
        
        
        public static bool ValidationsCountReportes(List<ReportesModel> Reportes)
        {

            if (Reportes.Count() <= 0)
            {
                return false;
            }

            return true;
        }



    }
}
