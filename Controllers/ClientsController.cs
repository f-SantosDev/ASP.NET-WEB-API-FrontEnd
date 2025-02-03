using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisao_ASP.NET_Web_API_Front.Models.ViewModels;

namespace Revisao_ASP.NET_Web_API_Front.Controllers
{
    public class ClientsController : Controller
    {
        // element to create and set HttpClient instances to auxiliar the frontend manipulate data through APIs
        public readonly IHttpClientFactory _httpClientFactory;

        // DI
        public ClientsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Read
        //[HttpGet("ClientList")]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false) // verify if the user is logedin (if is authenticated)
            {
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var clients = await clientConnection.GetFromJsonAsync<IEnumerable<ClientsViewModel>>("Clients/GetClients/AllClients"); // retriver data from API in JSON format / the string "GetClients" is the endpoint that will be access to - "api/GetClients", Action reference

                return View(clients); // return data retrived from API
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        // Read{id}
        [Route("ClientDetails/{clientId}")]
        public async Task<IActionResult> Details([FromRoute] int clientId)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

            var clientById = await clientConnection.GetFromJsonAsync<ClientsViewModel>($"Clients/GetClientById/{clientId}"); // retriver data from API in JSON format / the string "GetClientById" is the endpoint that will be access to - "api/GetClientById/{id}", Action reference

            return View(clientById);
        }

        //Create
        [Authorize(Roles = "Admin")] // define the level access to access this view
        // dont need use [HttpPost] and is not async because this Action will only build the view empty
        [HttpGet("CreateClient")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateClient")]
        public async Task<IActionResult> Create([FromForm] ClientsViewModel clientRegister)
        {
            if (ModelState.IsValid)
            {
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var createClient = await clientConnection.PostAsJsonAsync("Clients/AddClient", clientRegister); // create a new data client from API in JSON format / the string "AddClient" is the endpoint that will be access to - "api/AddClient", Action reference -- clientRegister data to create a new client

                if (createClient.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(clientRegister); // any error keep the actual view with actual data showing
        }

        // Update
        [Authorize(Roles = "Admin")] // define the level access to access this view
        [Route("UpdateClient/{clientId}")]
        public async Task<IActionResult> Update([FromRoute] int clientId)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

            var searchClient = await clientConnection.GetFromJsonAsync<ClientsViewModel>($"Clients/GetClientById/{clientId}");

            return View(searchClient);
        }

        [HttpPost]
        [Route("UpdateClient/{clientId}")]
        public async Task<IActionResult> Update([FromRoute] int clientId, [FromForm] ClientsViewModel clientRegister)
        {
            if (ModelState.IsValid) 
            {
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var updateClient = await clientConnection.PutAsJsonAsync($"Clients/UpdateClient/{clientId}", clientRegister);

                if (updateClient.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(clientRegister);
        }

        // Delete
        [Authorize(Roles = "Admin")] // define the level access to access this view
        [Route("DeleteClient/{clientId}")]
        public async Task<IActionResult> Delete([FromRoute] int clientId)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

            await clientConnection.DeleteAsync($"Clients/DeleteClient/{clientId}");

            return RedirectToAction("Index");
        }
    }
}
