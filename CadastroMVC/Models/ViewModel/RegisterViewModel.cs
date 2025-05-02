using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CadastroMVC.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo 'Nome' não pode estar vazio.")]
        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "Digite um nome válido.", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'CPF' não pode estar vazio.")]
        [Display(Name = "CPF")]
        [StringLength(50, ErrorMessage = "Digite um CPF válido.", MinimumLength = 11)]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo 'Telefone' não pode estar vazio.")]
        [Phone(ErrorMessage = "O campo deve estar no formato de telefone")]
        [Display(Name = "Telefone")]
        [StringLength(70, ErrorMessage = "Digite um telefone válido.", MinimumLength = 8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo 'Placa' não pode estar vazio.")]
        [Display(Name = "Placa")]
        [StringLength(50, ErrorMessage = "Digite uma Placa válida.", MinimumLength = 6)]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O campo 'Modelo' não pode estar vazio.")]
        [Display(Name = "Modelo")]
        [StringLength(50, ErrorMessage = "Digite um modelo válido.", MinimumLength = 2)]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O campo 'CEP' não pode estar vazio.")]
        [Display(Name = "CEP")]
        [StringLength(50, ErrorMessage = "Digite um CEP válido.", MinimumLength = 8)]
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
