using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class CadastroTypes : IdentityRole<int>
    {
        public List<CadastroUserTypes> UserTypes { get; set; }
    }
}
