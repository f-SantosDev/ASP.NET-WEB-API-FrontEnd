using Microsoft.AspNetCore.Mvc;

namespace Revisao_ASP.NET_Web_API_Front.ViewComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = HttpContext.User; // obtain user loged in

            return View(user); // send the user object to the view component
        }
    }
}
