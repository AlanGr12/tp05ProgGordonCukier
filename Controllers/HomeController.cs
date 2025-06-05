using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp05Gordon_Cukier.Models;

namespace tp05Gordon_Cukier.Controllers;

public class HomeController : Controller
{  
    private const string SessionJuegoKey = "Juego";
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult introduccion()
    {
        return View("introduccion");
    }

    public IActionResult segundaSala()
    {
        return View("segundaSala");
    }

    public IActionResult tercerSala(){
        return View("terceraSala");
    }

    [HttpGet]

    public IActionResult Validacion(){
        return View("validacion");
    }

    [HttpPost]

    public IActionResult Validacion(string nombreJugador){

        if(nombreJugador != null && nombreJugador != "")
        HttpContent.Session.SetString("nombreJugador", nombreJugador);
        HttpContent.Session.SetInt32("HabitacionAct", 1);

        ViewBag.nombreJugador = nombreJugador;
        ViewBag.numeroHabitacion = 1;
        return View("Habitacion");


    }
}
