using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;

namespace Transportador.Entidades
{



    public class Contexto : DbContext
    {
        public DbSet<Transportadora> Transportadora { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Classificacao> Classificacao { get; set; }

            


    }


}
