using CadastroMVC.Models;
using CadastroMVC.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace CadastroMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient Client;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
              _configuration = configuration;
              _clientFactory = clientFactory;
              Client = _clientFactory.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");
        }

        public async Task<IActionResult> Profile()
        {
            Client.DefaultRequestHeaders.Authorization = new
                AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await Client.GetAsync(Client.BaseAddress + _configuration.GetValue<string>("ConnectionEndPoints:Show-Associado"));
            if (response.IsSuccessStatusCode)
            {
                var associado = await response.Content.ReadFromJsonAsync<AssociadoViewModel>();
                TempData["associado_data"] = JsonConvert.SerializeObject(associado.Endereco);
                return View(associado);
            }
            else
            {
                return View("Login");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Submitlogin(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await Client.PostAsJsonAsync(Client.BaseAddress + _configuration.GetValue<string>("ConnectionEndPoints:Login"), loginViewModel);
                if (response.IsSuccessStatusCode)
                {
                    var returnValue = await response.Content.ReadAsStringAsync();
                    var returnToken = JsonConvert.DeserializeObject<JWT>(returnValue);
                    HttpContext.Session.SetString("token", returnToken.token);
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", 
                                                                 HttpContext.Session.GetString("token"));

                    return RedirectToAction("Profile", "User");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["Unauthorized"] = "CPF ou Placa incorretos.";
                    return View("Login");
                }
                return View("Errors", StatusCode(StatusCodes.Status500InternalServerError, "Serviço indisponível."));
            }
            return View("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> SubmitRegister(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await Client.PostAsJsonAsync(Client.BaseAddress + _configuration.GetValue<string>("ConnectionEndPoints:Register"), registerViewModel);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cadastro efetuado com sucesso! Entre com seu CPF e placa";
                    return View("Login");
                }
                return View("Errors", StatusCode(StatusCodes.Status500InternalServerError, "Serviço indisponível."));
            }
            return View("Register");
        }

        public IActionResult Update()
        {
            EnderecoViewModel associado = JsonConvert.DeserializeObject<EnderecoViewModel>((string)TempData["associado_data"]);
            return View(associado);
        }

        public async Task<IActionResult> SubmitUpdate(EnderecoViewModel enderecoViewModel)
        {
            if (ModelState.IsValid)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));

                HttpResponseMessage response = await Client.PutAsJsonAsync(Client.BaseAddress + _configuration.GetValue<string>("ConnectionEndPoints:Update"), enderecoViewModel);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Update"] = "Dados atualizados com sucesso!";
                    return RedirectToAction("Profile", "User");
                }
                return View("Errors", StatusCode(StatusCodes.Status500InternalServerError, "Serviço indisponível."));
            }
            return View("Update");
        }
    }
}
