using System.ComponentModel.DataAnnotations;

namespace Transportador.Models
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
        public class UsuarioModels
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome não pode ser branco.")]
        public string Nome { get; set; }
        public string email { get; set; }
        public string Senha { get; set; }


    }
}
