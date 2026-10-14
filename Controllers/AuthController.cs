
using Microsoft.AspNetCore.Mvc;

namespace CotizadorIA.Controllers;


public class AuthController : Controller
{
   [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}