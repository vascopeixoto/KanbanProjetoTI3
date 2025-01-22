using ProjetoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Task = ProjetoFinal.Models.Task;

namespace ProjetoFinal.Controllers
{
    public class TestController : Controller
    {
        private User? user;
        private UserService userService;
        private UserHelper userHelper;
        private TaskTester taskTester;
        private StagesHelper stagesHelper;

        string logFilePath =
            $"log{DateTime.UtcNow.Hour}{DateTime.UtcNow.Day}{DateTime.UtcNow.Month}{DateTime.UtcNow.Year}.txt";

        public TestController()
        {
            userService = new UserService();
            userHelper = new UserHelper();
            taskTester = new TaskTester();
            stagesHelper = new StagesHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext aec)
        {
            base.OnActionExecuting(aec);

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Program.SessionContainerName)))
            {
                HttpContext.Session.SetString(Program.SessionContainerName, "");
            }

            user = userService.GetBySession("" + HttpContext.Session.GetString(Program.SessionContainerName));
            if (user != null) ViewBag.Acc = user;
            else ViewBag.Acc = userService.SetGuest();
        }


        [HttpGet]
        public IActionResult TestTasks()
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            var stages = stagesHelper.List("" + HttpContext.Session.GetString(Program.SessionContainerName));

            DateTime Start = DateTime.Now;
            string ret = "";
            foreach (var stage in stages)
            {
                for (int contador = 0; contador < 1000; contador++)
                {
                    Task m = new Task
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = taskTester.geraTitle(),
                        Description = taskTester.geraDescricao(),
                        EstimatedTime = taskTester.GeraEstimatedDate(),
                        DateCreated = DateTime.Now,
                        UserId = user.Id,
                        StageId = stage.Id
                    };

                    ret = taskTester.Criar(m);
                    Logs.Write(logFilePath, "ID Task:" + ret);
                }
            }

            DateTime End = DateTime.Now;
            Logs.Write(logFilePath, "Demorou " + (End - Start).TotalSeconds + " a criar 1000 Tarefas por stage");
            //----------------------------------------------------------------------------------------------------
            //Teste Leitura Aleatoria
            //----------------------------------------------------------------------------------------------------
            DateTime StartRandom = DateTime.Now;
            List<Task> randTask = taskTester.GetRandom10PercentTask();
            Logs.Write(logFilePath, "\n---------------------\nLista de Tasks\n-------------------\n");
            foreach (Task m in randTask)
            {
                Logs.Write(logFilePath,
                    $"Nome: {m.Title}; Tempo estimado: {m.EstimatedTime}h; Stage: {m.Stage.Name}; Utilizador: {m.User.Name};\n");
            }

            DateTime EndRandom = DateTime.Now;
            Logs.Write(logFilePath, "Demorou " + (EndRandom - StartRandom).TotalSeconds +
                                    $" a ler 10% ({randTask.Count} Registos) de Tasks de forma aleatória");
            //----------------------------------------------------------------------------------------------------
            //Teste Update Aleatorio
            //----------------------------------------------------------------------------------------------------
            DateTime StartUpdate = DateTime.Now;
            foreach (Task m in randTask)
            {
                m.Title = taskTester.geraTitle();
                m.Description = taskTester.geraDescricao();
                taskTester.Atualizar(m);
            }

            DateTime EndUpdate = DateTime.Now;
            Logs.Write(logFilePath, "Demorou " + (EndUpdate - StartUpdate).TotalSeconds +
                                    $" a atualizar os 10% ({randTask.Count} Registos) de Tasks lidos de forma aleatória");
            //----------------------------------------------------------------------------------------------------

            return Ok();
        }
    }
}