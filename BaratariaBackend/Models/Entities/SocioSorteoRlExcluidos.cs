using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Models.Entities
{
    public class SocioSorteoRlExcluidos
    {
        public int IdSocio { get; set; }
        public int IdSorteo { get; set; }
    }
}
