using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transportador.Entidades
{

    [Table("Transportadora")]
    public class Transportadora
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome não pode ser branco.")]
        public string Nome { get; set; }


        public int ClassificacaoId { get; set; }
        public virtual Classificacao Classificacao { get; set; }
    }
}
