using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportador.Entidades;
using System.Data.Entity;

namespace Transportador.Negocio
{
    public class TransportadoraFacade
    {

        public bool Gravar(Transportadora entity)
        {
            bool ok = true;
            try
            {
                using (var db = new Contexto())
                {
                    db.Transportadora.Add(entity);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                ok = false;

                throw;
            }
            return ok;
        }

        public bool Excluir(int? id)
        {
            bool ok = true;
            return ok;
        }

        public IQueryable<Transportadora> Listar(string nome)
        {
            var db = new Contexto();


            return from i in db.Transportadora
                   where i.Nome == nome || nome == string.Empty
                   select i;


        }

        public IQueryable<Transportadora> Atualizar(string nome)
        {
            return null;

        }


    }
}
