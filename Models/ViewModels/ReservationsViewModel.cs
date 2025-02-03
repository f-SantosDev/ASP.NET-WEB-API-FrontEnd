using Revisao_ASP.NET_Web_API_Front.Models.Entities;

namespace Revisao_ASP.NET_Web_API_Front.Models.ViewModels
{
    public class ReservationsViewModel
    {
        public int ReservationId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }

        public int ClientId { get; set; }
        //public Clients? Client { get; set; }
        public ClientsViewModel? Client { get; set; }
    }
}
