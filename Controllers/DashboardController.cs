using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CotizadorIA.Controllers;

[Authorize(Roles = "Administrador")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View("~/Views/Home/Dashboard.cshtml");
    }
}
