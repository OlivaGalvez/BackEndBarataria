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
        [Column(TypeName = "timestamp")]
        public DateTime? FechaAlta { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? FechaInicio { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? FechaFin { get; set; }
        [Column(TypeName = "boolean")]
        public bool? Mostrar { get; set; }
        [Column(TypeName = "varchar")]
        public string Texto { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenOriginal { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenServidor { get; set; }
        [Column(TypeName = "bigint")]
        public long? ImagenPeso { get; set; }


        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<DireccionWeb> DireccionWebs { get; set; }
    }
}
