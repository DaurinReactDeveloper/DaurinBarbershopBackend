using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Validations
{
    public static class AuthenticationValidations
    {
        public static bool AuthenticationValidationPassword(string passwordUser, string Password)
        {

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(passwordUser, Password);

            if (!isPasswordValid)
            {

                return false;

            }

            return true;
        }

    }
}
