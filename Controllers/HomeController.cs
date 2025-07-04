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

        [HttpGet]
        public IActionResult salaIntro()
        {
            return View("salaIntro");
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

            return RedirectToAction("Sala", new { id = 1 });
        }

        public IActionResult Sala(int id)
        {
            var jugador = ObtenerJugador();
            if (jugador == null)
            {
                return RedirectToAction("salaIntro");
            }

            if (id > jugador.salaAct)
            {
                return RedirectToAction("Sala", new { id = jugador.salaAct });
            }

            ViewBag.Sala = id;
            ViewBag.NombreJugador = jugador.nombreJugador;
            ViewBag.Vidas = jugador.vidas;

            return View($"sala{id}");
        }

        [HttpPost]
        public IActionResult VerificarAcertijo(string respuesta, int salaActual)
        {
            var jugador = ObtenerJugador();
            if (jugador == null || salaActual != jugador.salaAct)
            {
                return RedirectToAction("salaIntro");
            }

            bool correcta = false;

            switch (salaActual)
            {
                case 1:
                    correcta = respuesta == "sombra";
                    break;
                case 2:
                    correcta = respuesta == "sol";
                    break;
                case 3:
                    correcta = respuesta == "letrero";
                    break;
                case 4:
                    correcta = respuesta == "5:00";
                    break;
                case 5:
                    correcta = respuesta== "5307";
                    break;

            }

            if (correcta)
            {
                string sala;
                jugador.salaAct++;
                GuardarJugador(jugador);

                if (jugador.salaAct == 6)
                    return RedirectToAction("ganaste");

                return RedirectToAction("Sala", new { id = jugador.salaAct });
            }
            else
            {
                jugador.vidas--;
                GuardarJugador(jugador);

                if (jugador.vidas <= 0)
                    return RedirectToAction("perdiste");

                ViewBag.NombreJugador = jugador.nombreJugador;
                ViewBag.Vidas = jugador.vidas;
                ViewBag.MensajeError = "Respuesta incorrecta. Perdés una vida.";

                return View($"sala{salaActual}");
            }
        }

        public IActionResult ganaste()
        {
            return View("ganaste");
        }

        public IActionResult Creditos()
        {
          
            return View("creditos");
        }
    public IActionResult perdiste()
    {
        return View("perdiste");
    }
    }
