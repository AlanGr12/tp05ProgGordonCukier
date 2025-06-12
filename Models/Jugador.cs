public class Jugador{
    [JsonProperty]

    public string nombre {get; set;}

    [JsonProperty]

    public int salaAct{get; set;} = 1;

     [JsonProperty]

     public int vidas{get; set;} = 3;
    
}