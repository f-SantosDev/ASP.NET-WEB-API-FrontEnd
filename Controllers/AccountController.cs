using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Revisao_ASP.NET_Web_API_Front.Models.ViewModels;
using System.Security.Claims;

namespace Revisao_ASP.NET_Web_API_Front.Controllers
{
    public class AccountController : Controller
    {
        // set DI
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //*************************************************************************************//
        //                                  Create new User                                   //
        //***********************************************************************************//
        //
        // Create a empty view to register a new user
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var createUser = await clientConnection.PostAsJsonAsync("Auth/Register", userRegister); // create a new user trhouth API endpoint Auth/Register

                if (createUser.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to register user");
                }
            }

            return View(userRegister);
        }

        //*************************************************************************************//
        //                                     Login User                                     //
        //***********************************************************************************//
        //
        // Create a empty view to login user
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel credentials)
        {
            if (ModelState.IsValid) 
            {
                var clientConnection = _httpClientFactory.CreateClient("TripsAPI"); // create the client (frontend) connection with API

                var userLogin = await clientConnection.PostAsJsonAsync("Auth/Login", credentials); // access API endpoint to login the user

                if (userLogin.IsSuccessStatusCode)
                {
                    // if the login is success we get the roles to set the access level
                    var rolesResponse = await clientConnection.GetAsync("Auth/GetRoles");

                    var roles = new List<string>();

                    if (rolesResponse.IsSuccessStatusCode)
                    {
                        roles = await rolesResponse.Content.ReadFromJsonAsync<List<string>>();
                    }

                    ///////////////////////////////////////////////////////////////////////////////////

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, credentials.Email)
                    };

                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); // set the roles to user claims list

                    var identity = new ClaimsIdentity(claims, "Identity.Application");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("Identity.Application", principal);

                    return RedirectToAction("Index", "Clients");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please verify your credentials.");
                }
            }

            return View(credentials);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("Identity.Application"); // Logout the user

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}