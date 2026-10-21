using Microsoft.AspNetCore.Mvc;

namespace CotizadorIA.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
