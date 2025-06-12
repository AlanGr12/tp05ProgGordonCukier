using Newtonsoft.Json;
public class Juego{


  public bool Validacion(string nombreJugador){

        if(nombreJugador != null && nombreJugador != "")
        HttpContent.Session.SetString("nombreJugador", nombreJugador);
        HttpContent.Session.SetInt32("HabitacionAct", 1);

        ViewBag.nombreJugador = nombreJugador;
        ViewBag.numeroHabitacion = 1;
        return View("Habitacion");


    }}