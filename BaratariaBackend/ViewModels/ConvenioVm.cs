using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.ViewModels
{
    public class ConvenioVm
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public bool? Mostrar { get; set; }
        public string ImagenServidor { get; set; }
        public string ImagenServidorBase64 { get; set; }
        public string Url { get; set; }
    }
}
