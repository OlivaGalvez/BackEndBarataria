using BaratariaBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.ViewModels
{
    public class AsociacionVm
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string Texto { get; set; }
        public List<Documento> ListDocumentos { get; set; }
    }
}
