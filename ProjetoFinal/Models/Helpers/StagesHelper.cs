using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class StagesHelper : HelperBase
{
    private StagesService stagesService;

    public StagesHelper()
    {
        stagesService = new StagesService();
    }

    public List<Stage> List()
    {
        try
        {
            return stagesService.List();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new();
        }
    }

    public void Save(Stage stage)
    {
        try
        {
            stagesService.Save(stage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
    
    public Stage? Get(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            
            return stagesService.GetById(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public void Delete(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return;
            
            stagesService.Delete(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
}