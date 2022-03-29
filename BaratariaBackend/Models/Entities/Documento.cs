using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class Documento
    {
        [Key]
        public int Id { get; set; }
        public int? ActividadId { get; set; }
        public int? SocioId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Nombre { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Original { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Servidor { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Url { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Tamanio { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? Fecha { get; set; }
        [Column(TypeName = "boolean")]
        public bool? Privado { get; set; }

        public virtual ICollection<TpDocumento> TpDocumentos { get; set; }
    }
}
