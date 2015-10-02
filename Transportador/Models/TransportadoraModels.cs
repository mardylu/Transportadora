using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Transportador.Models
{

    public class TransportadoraModels
    {
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Nome não pode ser branco.")]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage = "Informa sua Classificação.")]
        public int ClassificacaoId { get; set; }
        [Display(AutoGenerateField = true, AutoGenerateFilter = true, Description = "Classificação")]
        public virtual IList<ClassificacaoModels> Classificacao { get; set; }
    }
}
