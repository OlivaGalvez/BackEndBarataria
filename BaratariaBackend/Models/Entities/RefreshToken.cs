using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string IdUsuario { get; set; } 
        public string Token { get; set; }
        public string IdJwt { get; set; } 
        public bool EnUso { get; set; } // Si está en uso no generamos otro token
        public bool Revocado { get; set; } // Si ha sido eliminado por razones de seguridad
        public DateTime FechaAlta { get; set; } //Fecha de creación
        public DateTime FechaExpiracion { get; set; } // Fecha de expiración

        [ForeignKey(nameof(IdUsuario))]
        public IdentityUser User { get; set; }
    }
}
