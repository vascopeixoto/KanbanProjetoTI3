using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace ProjetoFinal.Models;

public class TaskHelper : HelperBase
{
    private TaskService taskService;
    private StagesService stagesService;
    private UserService userService;

    public TaskHelper()
    {
        taskService = new TaskService();
        stagesService = new StagesService();
        userService = new UserService();
    }

    public List<TaskList> List(string hash)
    {
        try
        {
            var user = userService.GetBySession(hash);
            List<TaskList> list = new();
            var tasks = taskService.List(user.Id);
            foreach (var task in tasks)
            {
                var obj = new TaskList
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    EstimatedTime = task.EstimatedTime,
                    Stage = new()
                };

                var stage = stagesService.GetById(task.StageId);
                obj.Stage = stage;

                list.Add(obj);
            }

            return list;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new();
        }
    }

    public void Save(TaskSave task, string hash)
    {
        try
        {
            var user = userService.GetBySession(hash);
            var stages = stagesService.List();

            var newTask = new Task();

            if (string.IsNullOrWhiteSpace(task.Id))
            {
                newTask = new Task
                {
                    Title = task.Title,
                    Description = task.Description,
                    EstimatedTime = task.EstimatedTime,
                    DateCreated = DateTime.UtcNow,
                    User = user,
                    Stage = stages.MinBy(x => x.Sort)
                };
            }
            else
            {
                newTask = new Task
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    EstimatedTime = task.EstimatedTime,
                };
            }

            taskService.Save(newTask);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    public TaskSave? Get(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var task = taskService.GetById(id);

            return new TaskSave
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                EstimatedTime = task.EstimatedTime
            };
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

            taskService.Delete(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }
}