using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Route("signin-oidc")]
        public IActionResult SingInOidc()
        {
            var req = Request;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MakeApiCall()
        {
            var client = new HttpClient();
            var access = await HttpContext.GetTokenAsync("access_token");
            var id = await HttpContext.GetTokenAsync("id_token");
            client.SetBearerToken(access);
            string message = "n\\a";
            try
            {
                message = await client.GetStringAsync("http://localhost:5001/api/identity");
            }
            catch (Exception e)
            {
                message = e.Message;}

            return new JsonResult(new { access, id, message });
        }
    }
}