namespace Transportador.Entidades.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Transportador.Entidades.Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Transportador.Entidades.Contexto";
        }

        protected override void Seed(Transportador.Entidades.Contexto context)
        {
          
            context.Classificacao.AddOrUpdate(
              p => p.Nome,
              new Classificacao { Nome = "Classifica 1" },
              new Classificacao { Nome = "Classifica 2" },
              new Classificacao { Nome = "Classifica 3" }
            );
            //
        }
    }
}
