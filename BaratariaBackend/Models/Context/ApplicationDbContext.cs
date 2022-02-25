using BaratariaBackend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<EnlaceActividad> EnlacesActividad { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<TpDocumento> TpDocumentos { get; set; }
        public DbSet<Enlace> Enlaces { get; set; }

        public DbSet<Sorteo> Sorteos { get; set; }
        public DbSet<TpSorteo> TpSorteos { get; set; }
        public DbSet<EnlaceSorteo> EnlacesSorteo { get; set; }
        public DbSet<SocioSorteoRlGanadores> SociosSorteoRlGanadores { get; set; }
        public DbSet<SocioSorteoRlExcluidos> SociosSorteoRlExcluidos { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Deporte> Deportes { get; set; }
        public DbSet<EnlaceDeporte> EnlacesDeporte { get; set; }

        // we override the OnModelCreating method here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SocioSorteoRlGanadores>().HasKey(vf => new { vf.IdSocio, vf.IdSorteo });
            modelBuilder.Entity<SocioSorteoRlExcluidos>().HasKey(vf => new { vf.IdSocio, vf.IdSorteo });
        }
    }
}
