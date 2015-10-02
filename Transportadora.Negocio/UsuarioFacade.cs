using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportador.Entidades;

namespace Transportador.Negocio
{
    public class UsuarioFacade
    {

        public bool Gravar(Usuario entity)
        {
            bool ok = true;
            try
            {
                using (var db = new Contexto())
                {
                    db.Usuario.Add(entity);
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

        public IQueryable<Usuario> Listar(string email, string senha)
        {
            using (var db = new Contexto())
            {

                return from i in db.Usuario
                       where i.Nome == email & i.Senha == senha
                       select i;


            }

        }

        public IQueryable<Usuario> Atualizar(string nome)
        {
            return null;

        }
    }
}
