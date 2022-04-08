using BaratariaBackend.Models.Entities;
using System;
using System.Collections.Generic;

namespace BaratariaBackend.ViewModels
{
    public class ConvenioVm
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public bool? Mostrar { get; set; }
        public string Texto { get; set; }
        public string ImagenServidor { get; set; }
        public string ImagenServidorBase64 { get; set; }
        public List<DireccionWeb> ListEnlaces { get; set; }
        public List<Documento> ListDocumentos { get; set; }
    }
}
