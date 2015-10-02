using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transportador.Entidades
{

    [Table("Classificacao")]
    public class Classificacao
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome não pode ser branco.")]
        public string Nome { get; set; }


        public virtual IQueryable<Transportadora> TranportadoraLista { get; set; }
        
    }
}
