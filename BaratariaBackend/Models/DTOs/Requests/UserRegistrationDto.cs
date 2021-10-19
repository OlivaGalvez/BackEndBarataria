using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.DTOs.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Contrasenia { get; set; }
    }
}
