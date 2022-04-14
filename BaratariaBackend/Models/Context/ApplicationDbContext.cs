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
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<DireccionWeb> DireccionWebs { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Enlace> Enlaces { get; set; }
        public DbSet<Convenio> Convenios { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }

        // we override the OnModelCreating method here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* modelBuilder.Entity<SocioSorteoRlGanadores>().HasKey(vf => new { vf.IdSocio, vf.IdSorteo });
            modelBuilder.Entity<SocioSorteoRlExcluidos>().HasKey(vf => new { vf.IdSocio, vf.IdSorteo });*/
        }
    }
}
