using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class Sorteo
    {
        [Key]
        public int Id { get; set; }
        public int IdTpSorteo { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Titulo { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime Hora { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime FechaBaja { get; set; }
        public bool? Mostrar { get; set; }
        [Column(TypeName = "varchar")]
        public string Texto { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenOriginal { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImagenServidor { get; set; }
        [Column(TypeName = "bigint")]
        public long ImagenPeso { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime FechaSorteo { get; set; }
        [Column(TypeName = "boolean")]
        public bool? Excluyente { get; set; }
        [Column(TypeName = "int")]
        public int DiasRepeticion { get; set; }

        public virtual ICollection<SocioSorteoRlGanadores> SocioSorteoRlGanadores { get; set; }
        public virtual ICollection<Socio> SocioSorteoRlExcluidos { get; set; }
    }
}
