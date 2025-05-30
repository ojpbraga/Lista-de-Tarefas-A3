using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class AssociadoDTO
    {
        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int Id { get; set; }

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

        [JsonIgnore]
        public VeiculoDTO Veiculo { get; set; }

        [JsonIgnore]
        public EnderecoDTO Endereco { get; set; }
    }
}