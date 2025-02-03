namespace Revisao_ASP.NET_Web_API_Front.Models.Entities
{
    public class Clients
    {
        public int ClientId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<Reservations>? Reservations;
    }
}
