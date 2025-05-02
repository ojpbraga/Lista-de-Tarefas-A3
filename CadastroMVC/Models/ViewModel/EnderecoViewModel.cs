using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CadastroMVC.Models.ViewModel
{
    public class EnderecoViewModel
    {
        [Required(ErrorMessage = "O campo 'CEP' não pode estar vazio.")]
        [Display(Name = "CEP")]
        [StringLength(50, ErrorMessage = "Digite um CEP válido.", MinimumLength = 8)]
        [DataType(DataType.PostalCode)]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O campo 'Rua' não pode estar vazio.")]
        [Display(Name = "Rua")]
        [StringLength(50, ErrorMessage = "Digite uma rua válida.", MinimumLength = 2)]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O campo 'Numero' não pode estar vazio.")]
        [Range(0, int.MaxValue, ErrorMessage = "Numero inválido.")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "O campo 'Bairro' não pode estar vazio.")]
        [Display(Name = "Bairro")]
        [StringLength(50, ErrorMessage = "Digite um bairro válido.", MinimumLength = 2)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo 'Cidade' não pode estar vazio.")]
        [Display(Name = "Cidade")]
        [StringLength(50, ErrorMessage = "Digite uma cidade válido.", MinimumLength = 2)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo 'Estado' não pode estar vazio.")]
        [Display(Name = "Estado")]
        [StringLength(50, ErrorMessage = "Digite um estado válido(nome completo).", MinimumLength = 3)]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O campo 'Pais' não pode estar vazio.")]
        [Display(Name = "Pais")]
        [StringLength(50, ErrorMessage = "Digite um país válido.", MinimumLength = 2)]
        public string Pais { get; set; }

        [Display(Name = "Observacao")]
        [StringLength(2000, ErrorMessage = "Digite uma observação válida.")]
        public string? Observacao { get; set; }
    }
}
