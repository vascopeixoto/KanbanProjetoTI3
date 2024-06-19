using ProjetoFinal.Models;

internal class Program
{
    public static string Connector = "";
    public static string Key = "";
    public static string IV = "";
    public static string SessionContainerName = "";

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSession(s  => s.IdleTimeout  = TimeSpan.FromMinutes(20));
        builder.Services.AddMvc();
        var config = builder.Configuration.GetSection("Configuration").Get<Configuration>();
        Connector = config!.Connection;
        Key = config!.Key;
        IV = config!.IV;
        SessionContainerName = "Acc";
        
        var app = builder.Build();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Kanban}/{action=List}/{op?}"
        );

        app.Run();
    }
}