using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class Imagen
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Nombre { get; set; }
        [Column(TypeName = "varchar")]
        public string Url { get; set; }

    }
}
