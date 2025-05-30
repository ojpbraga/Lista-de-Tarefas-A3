using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Constantes.Enum;

namespace Domain.Entities
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
        public int AssociadoId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(AssociadoId))]
        public Associado Associado { get; set; }
    }
}
