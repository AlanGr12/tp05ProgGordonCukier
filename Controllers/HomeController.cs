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

   private salaEscape ObtenerJugador()
        {
            string datos = HttpContext.Session.GetString("juego");
            return Objeto.convertirStringAObjeto<salaEscape>(datos);
        }

        private void GuardarJugador(salaEscape jugador)
        {
            string datos = Objeto.convertirObjetoAString(jugador);
            HttpContext.Session.SetString("juego", datos);
        }

    

        [HttpPost]
    
public IActionResult RegistrarJugador(string nombre)
{
    if (nombre == null)
    {
        ViewBag.Error = "Debes ingresar tu nombre.";
        return View("salaIntro");
    }

    salaEscape jugador = new salaEscape
    {
        nombreJugador = nombre,
        salaAct = 1,
        vidas = 3
    };

    GuardarJugador(jugador);

    
return RedirectToAction("primeraSala");
}

        public IActionResult Sala(int id)
        {
            var jugador = ObtenerJugador();
            if (jugador == null)
            {
                return View("salaIntro");
            }

            if (id > jugador.salaAct)
            {
                return View("salaIntro", new { id = jugador.salaAct });
            }

            ViewBag.Sala = id;
            ViewBag.Texto = ObtenerTextoSala(id);
            ViewBag.Pista = ObtenerPistaSala(id);
            return View("Sala" + id);
        }

        [HttpPost]
        public IActionResult SubirRespuesta(int salaId, string respuesta)
        {
            var jugador = ObtenerJugador();
            if (jugador == null || salaId != jugador.salaAct)
            {
                return View("salaIntro");
            }

            string clave = ObtenerClaveCorrecta(salaId);

            if (respuesta.ToLower() == clave)
            {
                jugador.salaAct++;
                GuardarJugador(jugador);

                if (salaId == 5)
                {
                    return View("Ganar");
                }

                return View("quintaSala", new { id = jugador.salaAct });
            }

            ViewBag.Error = "Respuesta incorrecta.";
            return View("salaIntro", new { id = salaId });
        }

    
       [HttpPost]
public IActionResult VerificarAcertijo(string respuesta, int salaActual)
{
    var jugador = ObtenerJugador();
    bool correcta = false;

    switch (salaActual)
    {
        case 2:
            correcta = respuesta.ToLower() == "sombra";
            break;
        case 3:
            correcta = respuesta.ToLower() == "sol";
            break;
        case 4:
            correcta = respuesta.ToLower() == "letrero";
            break;
            case 5:
          correcta = respuesta.ToLower() == "";
   
        default:
            break;
    }

    if (correcta)
    {
        jugador.salaAct++;
        GuardarJugador(jugador);
        return RedirectToAction($"sala{jugador.salaAct}"); 
    }
    else
    {
        jugador.vidas--;
        GuardarJugador(jugador);

        ViewBag.NombreJugador = jugador.nombreJugador;
        ViewBag.Vidas = jugador.vidas;
        ViewBag.MensajeError = "Respuesta incorrecta. Perdés una vida.";

        return View($"sala{salaActual}"); 
    }
}


  public IActionResult PrimeraSala()
{
    var jugador = ObtenerJugador();
    if (jugador == null)
    {
        return View("salaIntro"); 
    }
    ViewBag.NombreJugador = jugador.nombreJugador;
    return View();

    return View(); 
}
public IActionResult salaIntro()
{
    return View("salaIntro"); 
}
      
    }
