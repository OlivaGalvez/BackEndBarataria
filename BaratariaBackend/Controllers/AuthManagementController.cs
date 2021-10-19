using BaratariaBackend.Configuration;
using BaratariaBackend.Models.DTOs.Requests;
using BaratariaBackend.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using BaratariaBackend.Models.DTOs.Responses;
using BaratariaBackend.Models.Entities;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]")] //api/authManagement
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApplicationDbContext _apiDbContext;

        public AuthManagementController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, 
            IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParameters,
            ApplicationDbContext apiDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _apiDbContext = apiDbContext;
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

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var verificar = await VerifyToken(tokenRequest);

                if (verificar == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Token inválido"
                        },
                        Success = false
                    });
                }

                return Ok(verificar);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                "Invalid payload"
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

            var refreshToken = new RefreshToken()
            {
                IdJwt = token.Id,
                EnUso = false,
                IdUsuario = user.Id,
                FechaAlta = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddDays(7),
                Revocado = false,
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validación 1 - Formato token JWT
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, 
                    out var validatedToken);

                // Validación 2 - Validar algoritmo de cifrado
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validación 3 - Validar fecha de caducidad
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "El token aún no ha caducado"
                        }
                    };
                }

                // Validación 4 - Validar la existencia del token
                var storedToken = await _apiDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "El token no existe"
                        }
                    };
                }

                // Validación 5 - Validar si está en uso
                if (storedToken.EnUso)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "El token está en uso"
                        }
                    };
                }

                // Validación 6 - Validar si está revocado
                if (storedToken.Revocado)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "El token ha sido revocado"
                        }
                    };
                }

                // Validación 7 - Validar el id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.IdJwt != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "El token no coincide"
                        }
                    };
                }

                // Actualizar el token actual
                storedToken.EnUso = true;
                _apiDbContext.RefreshTokens.Update(storedToken);
                await _apiDbContext.SaveChangesAsync();

                // Generar nuevo token
                var dbUser = await _userManager.FindByIdAsync(storedToken.IdUsuario);
                return await GenerarJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Token caducado"))
                {

                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "El Token ha expirado, vuelve a logearte"
                        }
                    };

                }
                else
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Algo salió mal"
                        }
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        protected string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
