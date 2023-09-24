using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eStoreClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly string email;
        private readonly string password;
        public AccountController(IConfiguration configuration)
        {
            email = configuration["Account:Email"];
            password = configuration["Account:Password"];
        }
        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string ReturnUrl = null)
        {
            if (email == this.email && password == this.password)
            {
                var claims = new List<Claim>{ new Claim(ClaimTypes.Name, email)};

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                   
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                if (ReturnUrl == null) ReturnUrl = "~/Products";
                return LocalRedirect("" + ReturnUrl);
            }
            ViewBag.msg = "Wrong email or password";
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return SignOut(new AuthenticationProperties { RedirectUri = "/Login" },
            //    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
