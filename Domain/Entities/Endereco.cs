using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Endereco
    {
        public int Id { get; set; }
        public string CEP { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string? Observacao { get; set; }
        public int AssociadoId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(AssociadoId))]
        public Associado Associado { get; set; }
    }
}
