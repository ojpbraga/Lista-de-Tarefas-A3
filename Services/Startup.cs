using Application;
using Application.DTOs;
using Application.Interfaces;
using Data.Context;
using Data.Repository;
using Data.Repository.Identity;
using Domain.Identity;
using Domain.Intefaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CadastroWebApi
{
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<CadastroContext>();

            IdentityBuilder builder = services.AddIdentityCore<CadastroUser>(options => {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(CadastroTypes), builder.Services);
            builder.AddEntityFrameworkStores<CadastroContext>();
            builder.AddDefaultTokenProviders();
            builder.AddRoleValidator<RoleValidator<CadastroTypes>>();
            builder.AddRoleManager<RoleManager<CadastroTypes>>();
            builder.AddSignInManager<SignInManager<CadastroUser>>();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetValue<string>("Token"))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddTransient<IAssociadoRepository, AssociadoRepository>();
            services.AddTransient<IVeiculoRepository, VeiculoRepository>();
            services.AddTransient<IEnderecoRepository, EnderecoRepository>();
            services.AddTransient<ICadastroUserRepository, CadastroUserRepository>();

            services.AddTransient<IAssociadoApplication, AssociadoApplication>();
            services.AddTransient<IVeiculoApplication, VeiculoApplication>();
            services.AddTransient<IEnderecoApplication, EnderecoApplication>();
            services.AddTransient<ICadastroUserApplication, CadastroUserApplication>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHATEOAS(options =>
            {
                //Associado
                options.AddLink<AssociadoDTO>("self",
                    "Get-Associado",
                    HttpMethod.Get,
                    (x) => new { id = x.Id });

                options.AddLink<AssociadoDTO>("todos-associados",
                   "Get-Associados",
                   HttpMethod.Get,
                   null);

                options.AddLink<AssociadoDTO>("deletar-associado",
                    "Delete-Associado",
                    HttpMethod.Delete,
                    (x) => new { id = x.Id });

                options.AddLink<AssociadoDTO>("editar-associado",
                    "Edit-Associado",
                    HttpMethod.Put,
                    (x) => new { id = x.Id });

                options.AddLink<AssociadoDTO>("criar-associado",
                    "Create-Associado",
                    HttpMethod.Post,
                    null);

                options.AddLink<AssociadoDTO>("mostrar-associado",
                    "Own-Show",
                    HttpMethod.Get,
                    null);

                options.AddLink<AssociadoDTO>("atualizarEndereço-associado",
                    "Own-Update",
                    HttpMethod.Put,
                    null);

                //Endereco
                options.AddLink<EnderecoDTO>("self",
                    "Get-Endereco",
                    HttpMethod.Get,
                    (x) => new { id = x.Id });

                options.AddLink<EnderecoDTO>("todos-enderecos",
                    "Get-Enderecos",
                    HttpMethod.Get,
                    null);

                options.AddLink<EnderecoDTO>("editar-endereco",
                    "Edit-Endereco",
                    HttpMethod.Put,
                    (x) => new { id = x.Id });

                options.AddLink<EnderecoDTO>("criar-endereco",
                    "Create-Endereco",
                    HttpMethod.Post,
                    null);

                options.AddLink<EnderecoDTO>("deletar-endereco",
                    "Delete-Endereco",
                    HttpMethod.Delete,
                    (x) => new { id = x.Id });

                //Veiculos
                options.AddLink<VeiculoDTO>("todos-veiculos",
                    "Get-Veiculos",
                    HttpMethod.Get,
                    null);

                options.AddLink<VeiculoDTO>("self",
                    "Get-Veiculo",
                    HttpMethod.Get,
                    (x) => new { id = x.Id });

                options.AddLink<VeiculoDTO>("editar-veiculo",
                    "Edit-Veiculo",
                    HttpMethod.Put,
                    (x) => new { id = x.Id });

                options.AddLink<VeiculoDTO>("criar-veiculo",
                    "Create-Veiculo",
                    HttpMethod.Post,
                    null);

                options.AddLink<VeiculoDTO>("deletar-veiculo",
                    "Delete-Veiculo",
                    HttpMethod.Delete,
                    (x) => new { id = x.Id });
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.MapControllers();
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment enviroment);
        void ConfigureServices(IServiceCollection services);
    }
    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<T>(this WebApplicationBuilder webApplicationBuilder) where T : IStartup
        {
            var startup = Activator.CreateInstance(typeof(T), webApplicationBuilder.Configuration) as IStartup;
            if (startup == null)
            {
                throw new ArgumentException("Classe startup indisponível.");
            }

            startup.ConfigureServices(webApplicationBuilder.Services);

            var app = webApplicationBuilder.Build();

            startup.Configure(app, app.Environment);
            app.Run();

            return webApplicationBuilder;
        }
    }
}
