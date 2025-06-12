using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Core
{
    public interface IEmailServices
    {

        void SendEmail(string to, string subject, string body, bool isHtml = false);
        string RenderTemplateCita(string templateName, EmailModel model);
        string RenderTemplateCliente(string templateName, EmailModel model);
        string RenderTemplateBarbero(string templateName, EmailModel model);
        string RenderTemplateAdminSucursal(string templateName, EmailModel model);
        string LoadEmbeddedTemplate(string templateName);
        EmailModel GenerateEmailModelCita(string NombreCliente, string Estado, DateTime FechaCita, TimeSpan HoraCita, string? NombreBarbero = null);
        EmailModel GenerateEmailModelCliente(string NombreCliente, string Password);
        EmailModel GenerateEmailModelAdminSucursal(string NombreAdminSucursal,string NombreBarberia);
        EmailModel GenerateEmailModelBarbero(string NombreBarbero, string Password, string NombreBarberia);

    }
}
