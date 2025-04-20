using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAPP.Pages
{
    public class AutentificacionOTPModel : PageModel
    {
        public void OnGet(string email)
        {
            ViewData["EmailUsuario"] = email;
        }
    }
}
