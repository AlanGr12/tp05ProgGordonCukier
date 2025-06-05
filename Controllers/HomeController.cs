using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp05Gordon_Cukier.Models;

namespace tp05Gordon_Cukier.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult salas(){
        
    }
}
