using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class Actividad
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Titulo { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Hora { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaBaja { get; set; }
        public bool? Mostrar { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Texto { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenOriginal { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenServidor { get; set; }
        public long ImagenPeso { get; set; }

        public virtual ICollection<Enlace> Enlaces { get; set; }
        public virtual ICollection<Documento> Documentos { get; set; }
    }
}
