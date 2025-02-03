using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisao_ASP.NET_Web_API_Front.Models.ViewModels;

namespace Revisao_ASP.NET_Web_API_Front.Controllers
{
    public class ReservationsController : Controller
    {
        // element to create and set HttpClient instances to auxiliar the frontend manipulate data through APIs
        public readonly IHttpClientFactory _httpClientFactory;

        // DI
        public ReservationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var reservations = await clientConnection.GetFromJsonAsync<IEnumerable<ReservationsViewModel>>("Reservations/GetReservations/AllReservations");

                return View(reservations);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [Route("ReservationDetails/{reservationId}")]
        public async Task<IActionResult> Details([FromRoute] int reservationId)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

            var reservationById = await clientConnection.GetFromJsonAsync<ReservationsViewModel>($"Reservations/GetReservationById/{reservationId}");

            if(reservationById == null)
            {
                return NotFound();
            }

            // if we get any problem to bring the client data related to the reservation - below we can take the client data by manual
            if (reservationById.Client == null)
            {
                reservationById.Client = await clientConnection.GetFromJsonAsync<ClientsViewModel>($"Clients/GetClientById/{reservationById.ClientId}");
            }

            return View(reservationById);
        }

        [Authorize(Roles = "Admin")] // define the level access to access this view
        [Route("CreateReservation")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateReservation")]
        public async Task<IActionResult> Create([FromForm] ReservationsViewModel reservationRegister)
        {
            if (ModelState.IsValid)
            { 
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var createReservation = await clientConnection.PostAsJsonAsync<ReservationsViewModel>("Reservations/AddReservation", reservationRegister);

                if (createReservation.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(reservationRegister); // any error keep the actual view with actual data showing
        }

        [Authorize(Roles = "Admin")] // define the level access to access this view
        [Route("UpdateReservation/{reservationId}")]
        public async Task<IActionResult> Update([FromRoute] int reservationId)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

            var searchReservation = await clientConnection.GetFromJsonAsync<ReservationsViewModel>($"Reservations/GetReservationById/{reservationId}");

            return View(searchReservation);
        }

        [HttpPost]
        [Route("UpdateReservation/{reservationId}")]
        public async Task<IActionResult> Update([FromRoute] int reservationId, [FromForm] ReservationsViewModel reservationRegister)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

            var updateReservation = await clientConnection.PutAsJsonAsync($"Reservations/UpdateReservation/{reservationId}", reservationRegister);

            if (updateReservation.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(reservationRegister); // any error keep the actual view with actual data showing
        }

        [Authorize(Roles = "Admin")] // define the level access to access this view
        [Route("DeleteReservation/{reservationId}")]
        public async Task<IActionResult> Delete([FromRoute] int reservationId)
        {
            var clientConnection = _httpClientFactory.CreateClient("TripsAPI");

            await clientConnection.DeleteAsync($"Reservations/DeleteReservation/{reservationId}");

            return RedirectToAction("Index");
        }
    }
}
