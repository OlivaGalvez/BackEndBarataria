using BaratariaBackend.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaratariaBackend.ViewModels
{
    public class ActividadVm
    {
        [Key]
        public int Id { get; set; }
        public int? IdTpActividad { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Mostrar { get; set; }
        public string Texto { get; set; }
        public string ImagenServidor { get; set; }
        public string ImagenServidorBase64 { get; set; }
        public List<EnlaceActividad> ListEnlaces { get; set; }
        public List<Documento> ListDocumentos { get; set; }
    }
}
