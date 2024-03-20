using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CentaurosBackAPI.Models;
using Microsoft.EntityFrameworkCore;
using CentaurosData.DTOS;
using CentaurosData.Response;

namespace CentaurosBackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly CentaurosContext _context;
        private readonly IConfiguration _configuration;

        public AutenticacionController(CentaurosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Validar")]
        public async Task<IActionResult> Validar([FromBody] UsuarioDTO usuarioForm)
        {
            Respuesta<object> respuesta = new Respuesta<object>();

            try
            {
                // Validar entrada
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Buscar el usuario en la base de datos por correo electrónico
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(e => e.Usuario1 == usuarioForm.Correo);

                // Verificar si el usuario existe
                if (usuarioDB != null)
                {
                    // Verificar si la contraseña coincide
                    if (usuarioDB.Contraseña == usuarioForm.Clave)
                    {
                        // Generar el token JWT
                        var token = GenerarTokenJWT(usuarioDB.Usuario1);

                        respuesta.Exito = 1;
                        respuesta.Mensaje = "El usuario ha iniciado sesión exitosamente";
                        respuesta.Data = token;

                        // Devolver el token JWT como respuesta
                        return Ok(respuesta);

                    }
                    else
                    {
                        respuesta.Mensaje = "Contraseña incorrecta, por favor intente nuevamente.";
                        return Unauthorized(respuesta);
                    }
                }
                else
                {
                    respuesta.Mensaje = "El usuario ingresado no se encuentra registrado.";
                    return Unauthorized(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = $"Error interno del servidor: {ex.Message}";
                return StatusCode(500, respuesta);
            }
        }


        private string GenerarTokenJWT(string correo)
        {
            // Configurar la clave secreta y otros parámetros del token
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationMinutes"]));


            // Crear los claims del token
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, correo),
            };

            // Configura la descripción del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = signingCredentials
            };

            // Crear el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Serializar el token JWT a una cadena
            return tokenHandler.WriteToken(token);
        }
    }

}
