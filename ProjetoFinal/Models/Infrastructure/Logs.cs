namespace ProjetoFinal.Models;

public class Logs
{
    public static void Write(string filePath, string message)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(DateTime.Now + ": " + message);
        }
    }
}