using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class Enlace
    {
        [Key]
        public int Id { get; set; }
        public int ActividadId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Nombre { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Url { get; set; }

    }
}
