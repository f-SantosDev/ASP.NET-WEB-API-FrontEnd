﻿namespace Revisao_ASP.NET_Web_API_Front.Models.Entities
{
    public class Login
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
