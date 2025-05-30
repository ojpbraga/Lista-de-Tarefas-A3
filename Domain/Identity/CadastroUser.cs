using System.Text.Json.Serialization;
using Domain.Constantes.Enum;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class CadastroUser : IdentityUser<int>
    {
        //Associado
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }

        //Endereço
        public string CEP { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string? Observacao { get; set; }

        //Veiculo
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }

        public int? AssociadoId { get; set; }

        [JsonIgnore]
        public Associado Associado { get; set; }
    }
}
