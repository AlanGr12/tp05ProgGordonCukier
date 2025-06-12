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

   private Jugador obtenerJugador()
{
    string jugadorString = HttpContext.Session.GetString("Jugador");
    if (jugadorString == null)
    {
        return null;
    }
    return Objeto.convertirStringAObjeto<Jugador>(jugadorString);
}

    private void guardarJugadorSession(Jugador jugador)
    {
        HttpContext.Session.SetString("player", Objeto.convertirObjetoAString(jugador));
    }

    
    [HttpPost]
    public IActionResult registrarJugador(string nombreJugador)
{
        if (nombreJugador == null)
        {
            ViewBag.Error = "Registra tu nombre";
            return View("~/Views/Home/Index.cshtml"); 
        }

        Jugador nuevoJugador = new Jugador { nombre = nombreJugador, salaAct = 1 };
        guardarJugadorSession(nuevoJugador);
        return View("sala", new { id = 1 }); 
}

    
    public IActionResult sala(int id)
    {
        Jugador jugador = obtenerJugador();
        if (jugador == null)
        {
            return View("Index", "Home"); 
        }

        
        if (id > jugador.salaAct)
        {
            return View("sala", new { id = jugador.salaAct});
        }

        
        switch (id)
        {
            case 1:
                ViewBag.Texto = "Caiste en una cueva, la caída te dejo a poca vida y solo tienes una antorcha.";
                ViewBag.Adivinanza = "choice";
                return View("introduccion");
            case 2:
                ViewBag.Texto = "Lograste avanzar un poco, pero el camino esta bloqueado por troncos de madera, necesitas un hacha.";
                ViewBag.Adivinanza = "text";
                ViewBag.Pista = "Piensa en herramientas para cortar.";
                return View("segundaSala");
            case 3:
                ViewBag.Texto = "La siguiente puerta necesita un código, encuentra las pistas.";
                ViewBag.Adivinanza = "text";
                ViewBag.Pista = "Mira las marcas en la pared.";
                return View("terceraSala");
            case 4:
                ViewBag.Texto = "Hay una mesa de crafteo enfrente, pero cuidado, hay un warden cerca.";
                ViewBag.Adivinanza = "choice";
                return View("cuartaSala");
            case 5:
                ViewBag.Texto = "Fabrica algo con tu hierro.";
                ViewBag.Adivinanza = "text";
                ViewBag.Pista = "Algo para minar.";
                return View("quintaSala");
            case 6:
                ViewBag.Texto = "Ingresa la contraseña para que el vagon reciba energia.";
                ViewBag.Adivinanza = "text";
                ViewBag.Pista = "Qué quieres hacer?";
                return View("sala6");
            default:
                return View("introduccion", "Home"); 
        }
    }

    [HttpPost]
    public IActionResult subirRespuesta(int salaId, string respuesta)
    {
        Jugador jugador = obtenerJugador();
        if (jugador == null || salaId != jugador.salaAct)
        {
            return RedirectToAction("Index", "Home"); 
        }

        string respuestaCorrecta = "";
        bool correcta = false;

        switch (salaId)
        {
            case 1: 
                respuestaCorrecta = "avanzar"; 
                correcta = respuesta.ToLower() == respuestaCorrecta;
                break;
            case 2: 
                respuestaCorrecta = "hacha";
                correcta = respuesta.ToLower() == respuestaCorrecta;
                break;
            case 3: 
                respuestaCorrecta = "1234"; 
                correcta = respuesta == respuestaCorrecta;
                break;
            case 4: 
                respuestaCorrecta = "agachado";
                correcta = respuesta.ToLower() == respuestaCorrecta;
                break;
            case 5: 
                respuestaCorrecta = "pico";
                correcta = respuesta.ToLower() == respuestaCorrecta;
                break;
            case 6: 
                respuestaCorrecta = "escapar"; 
                correcta = respuesta.ToLower() == respuestaCorrecta;
                break;
        }

        if (correcta)
        {
            if (salaId == 6) 
            {
                return View("Ganar");
            }
            else
            {
                jugador.salaAct++;
                guardarJugadorSession(jugador);
                return View("sala", new { id = jugador.salaAct });
            }
        }
        else
        {
            ViewBag.Error = "Respuesta incorrecta, intenta de nuevo.";
          
            return sala(salaId);
        }
    }

    public IActionResult Ganar()
    {
        Jugador jugador = obtenerJugador();
        if (jugador == null)
        {
            return View("Index", "Home");
        }
        
        HttpContext.Session.Clear();
        return View();
    }
}