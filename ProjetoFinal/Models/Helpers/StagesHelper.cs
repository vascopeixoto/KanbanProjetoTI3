using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class StagesHelper : HelperBase
{
    private StagesService stagesService;
    private UserService userService;

    public StagesHelper()
    {
        stagesService = new StagesService();
        userService = new UserService();
    }

    public List<Stage> List(string hash)
    {
        try
        {
            var user = userService.GetBySession(hash);

            return stagesService.List(user.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new();
        }
    }

    public void Save(Stage stage, string hash)
    {
        try
        {
            var user = userService.GetBySession(hash);
            stage.UserId = user.Id;
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