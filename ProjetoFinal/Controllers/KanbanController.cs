using ProjetoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjetoFinal.Controllers
{
    public class KanbanController : Controller
    {
        private User? user;
        private UserService userService;
        private UserHelper userHelper;
        private TaskHelper taskHelper;

        public KanbanController()
        {
            userService = new UserService();
            userHelper = new UserHelper();
            taskHelper = new TaskHelper();
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
        public IActionResult List()
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");
            
            var tasks = taskHelper.List();

            return View(tasks);
        }
        
        
        [HttpGet]
        public IActionResult Create()
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            return View();
        }


        [HttpPost]
        public IActionResult Create(TaskSave task)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            taskHelper.Save(task, "" + HttpContext.Session.GetString(Program.SessionContainerName));

            return RedirectToAction("List", "Stages");
        }

        [HttpGet]
        public IActionResult Edit(string op)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            var task = taskHelper.Get(op);
            if (task is null)
                return RedirectToAction("List", "Stages");

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskSave task)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            taskHelper.Save(task, "" + HttpContext.Session.GetString(Program.SessionContainerName));

            return RedirectToAction("List", "Stages");
        }

        [HttpGet]
        public IActionResult Delete(string op)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            taskHelper.Delete(op);
            
            return RedirectToAction("List", "Stages");
        }
    }
}