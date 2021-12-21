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
        public int ActividadId { get; set; }
        public int SocioId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Original { get; set; }
        public DateTime? Fecha { get; set; }
        public bool? Privado { get; set; }

        public virtual ICollection<TpDocumento> TpDocumentos { get; set; }
    }
}
