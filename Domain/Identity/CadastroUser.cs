using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class CadastroUser : IdentityUser<int>
    {
        public string CPF { get; set; }
        public string Placa { get; set; }
        public int AssociadoId { get; set; }
    }
}
