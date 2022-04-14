using BaratariaBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.ViewModels
{
    public class OfertaVm
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Mostrar { get; set; }
        public string ImagenServidor { get; set; }
        public string ImagenServidorBase64 { get; set; }
        public List<Documento> ListDocumentos { get; set; }
    }
}
