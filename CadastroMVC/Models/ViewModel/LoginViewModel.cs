using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CadastroMVC.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo 'CPF' não pode estar vazio.")]
        [Display(Name = "CPF")]
        [StringLength(50, ErrorMessage = "Digite um CPF válido.", MinimumLength = 11)]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo 'Placa' não pode estar vazio.")]
        [Display(Name = "Placa")]
        [StringLength(50, ErrorMessage = "Digite uma Placa válida.", MinimumLength = 6)]
        public string Placa { get; set; }
    }
}
