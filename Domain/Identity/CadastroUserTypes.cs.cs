using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class CadastroUserTypes : IdentityUserRole<int>
    {
        public CadastroUser CadastroUser { get; set; }
        public CadastroTypes CadastroTypes { get; set; }
    }
}
