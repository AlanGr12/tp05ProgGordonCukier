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
            correcta = respuesta.ToLower() == "agujero";
            break;
        case 3:
            correcta = respuesta.ToLower() == "sombra";
            break;
        case 4:
            correcta = respuesta.ToLower() == "fuego";
            break;
        // agregás más salas y respuestas acá
        default:
            break;
    }

    if (correcta)
    {
        jugador.salaAct++;
        GuardarJugador(jugador);
        return RedirectToAction($"sala{jugador.salaAct}"); // va a sala3, sala4, etc.
    }
    else
    {
        jugador.vidas--;
        GuardarJugador(jugador);

        ViewBag.NombreJugador = jugador.nombreJugador;
        ViewBag.Vidas = jugador.vidas;
        ViewBag.MensajeError = "Respuesta incorrecta. Perdés una vida.";

        return View($"sala{salaActual}"); // vuelve a la misma sala
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
        private string ObtenerTextoSala(int id)
        {
            switch (id)
            {
                case 1: return "Estás en la entrada de la cueva.";
                case 2: return "Unos troncos bloquean el camino.";
                case 3: return "Una puerta con combinación bloquea el paso.";
                case 4: return "Cuidado con el warden. Hay una mesa de crafteo.";
                case 5: return "Fabricá algo con el hierro para poder escapar.";
                default: return "";
            }
        }

        private string ObtenerPistaSala(int id)
        {
            switch (id)
            {
                case 1: return "Escribí 'avanzar'.";
                case 2: return "¿Qué corta madera?";
                case 3: return "Fijate en las marcas del muro.";
                case 4: return "No hagas ruido.";
                case 5: return "Se usa para minar.";
                default: return "";
            }
        }

        private string ObtenerClaveCorrecta(int id)
        {
            switch (id)
            {
                case 1: return "avanzar";
                case 2: return "hacha";
                case 3: return "1234";
                case 4: return "agachado";
                case 5: return "pico";
                default: return "";
            }
        }
    }
