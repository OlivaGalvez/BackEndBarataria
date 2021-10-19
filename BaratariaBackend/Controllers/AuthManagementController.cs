using BaratariaBackend.Configuration;
using BaratariaBackend.Models.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using BaratariaBackend.Models.DTOs.Responses;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]")] //api/authManagement
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, 
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existeUsuario = await _userManager.FindByEmailAsync(user.Email);

                if (existeUsuario != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "El email ya está en uso"
                        },
                        Success = false
                    });
                }

                var nuevoUsuario = new IdentityUser()
                {
                    UserName = user.Nombre,
                    Email = user.Email
                };

                var usuarioCreado = await _userManager.CreateAsync(nuevoUsuario, user.Contrasenia);

                if (usuarioCreado.Succeeded)
                {
                    //Añadir Rol
                    var obtenerUsuario = await _userManager.FindByEmailAsync(user.Email);

                    bool existingRole = await _roleManager.RoleExistsAsync(user.Rol);
                    if (existingRole)
                    {
                        var resultadoRol = await _userManager.AddToRoleAsync(obtenerUsuario, user.Rol);

                        return Ok(await GenerarJwtToken(nuevoUsuario));
                    }

                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "El rol añadido no existe"
                        },
                        Success = false
                    });

                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = usuarioCreado.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }

            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                    "Datos erroneos"
                },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var usuarioExiste = await _userManager.FindByEmailAsync(user.Email);

                if (usuarioExiste == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Email o contraseña no válido"
                        },
                        Success = false
                    });
                }

                var usuarioEsCorrecto = await _userManager.CheckPasswordAsync(usuarioExiste, user.Contrasenia);

                if (!usuarioEsCorrecto)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Email o contraseña no válido"
                        },
                        Success = false
                    });
                }

                return Ok(await GenerarJwtToken(usuarioExiste));
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                    "Datos erroneos"
                },
                Success = false
            });
        }

        private async Task<AuthResult> GenerarJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true
            };
        }
    }
}
