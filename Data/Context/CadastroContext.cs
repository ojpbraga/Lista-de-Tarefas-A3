using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class CadastroContext : IdentityDbContext<CadastroUser, CadastroTypes, int,
                                                     IdentityUserClaim<int>, CadastroUserTypes,
                                                     IdentityUserLogin<int>, IdentityRoleClaim<int>,
                                                     IdentityUserToken<int>>
    {
        protected readonly IConfiguration _configuration;
        public CadastroContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Associado> Associados { get; set; }
        public virtual DbSet<Carro> Carros { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var connection = _configuration.GetSection("ConnectionStrings:default").Value;
                optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
            }
        }
    }
}