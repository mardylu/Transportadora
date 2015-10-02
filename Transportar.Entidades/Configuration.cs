using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportador.Entidades
{
    internal sealed class Configuration: DbMigrationsConfiguration<Contexto>
    {

        public Configuration() {

            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Contexto context)
        {
            base.Seed(context);
        }

    }
}
