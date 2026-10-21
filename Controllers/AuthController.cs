using Microsoft.AspNetCore.Mvc;
using CotizadorIA.Models;
using CotizadorIA.Data;
using System.Data.Common;
namespace CotizadorIA.Controllers;


public class AuthController : Controller
{
    private readonly ApplicationDbContext db;

    public AuthController(ApplicationDbContext db)
    {
        this.db = db;   
    }

   [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Process login
            var user = db.Usuarios.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                // User is authenticated
                return RedirectToAction("Create", "Usuario");
            }
            else
            {
                // Invalid credentials
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return View();
    }
}