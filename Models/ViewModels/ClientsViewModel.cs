using Revisao_ASP.NET_Web_API_Front.Models.Entities;

namespace Revisao_ASP.NET_Web_API_Front.Models.ViewModels
{
    public class ClientsViewModel
    {
        public int ClientId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        //public ICollection<Reservations>? Reservations;

        public List<ReservationsViewModel> Reservations { get; set; } = new List<ReservationsViewModel>();
    }
}
