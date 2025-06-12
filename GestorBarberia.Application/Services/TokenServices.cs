using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestorBarberia.Application.Services
{
    public static class TokenServices
    {
        public static string GenerateToken(string name, string role, IConfiguration configuracion)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //claims - las afirmaciones de mi token
            var claim = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            //Configurar el token 
            var token = new JwtSecurityToken(
                issuer: configuracion["Jwt:Issuer"],
                audience: configuracion["Jwt:Audience"],
                claims: claim,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
