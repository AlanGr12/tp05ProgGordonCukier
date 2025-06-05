public static class Objeto
{      public static string convertirObjetoAString<T>(T? obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    public static T? convertirStringAObjeto <T>(string txt)
    {
        if(string.IsNullOrEmpty(txt))
        {
            return default;
        }
        else 
        {
            return JsonConvert.DeserializeObject<T>(txt);
        }
    } 
}





