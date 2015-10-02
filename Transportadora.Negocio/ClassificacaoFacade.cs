using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportador.Entidades;

namespace Transportador.Negocio
{
  public  class ClassificacaoFacade
    {

        public IQueryable<Classificacao> Listar()
        {
            var db = new Contexto();


            return from i in db.Classificacao
                   select i;

            

        }



    }
}
