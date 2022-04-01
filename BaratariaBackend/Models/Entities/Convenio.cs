using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaratariaBackend.Models.Entities
{
    public class Convenio
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Titulo { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? FechaAlta { get; set; }
        [Column(TypeName = "boolean")]
        public bool? Mostrar { get; set; }
        [Column(TypeName = "varchar")]
        public string ImagenOriginal { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenServidor { get; set; }
        [Column(TypeName = "bigint")]
        public long? ImagenPeso { get; set; }
        [Column(TypeName = "varchar")]
        public string Url { get; set; }
    }
}
