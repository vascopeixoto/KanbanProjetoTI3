using ProjetoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjetoFinal.Controllers
{
    public class StagesController : Controller
    {
        private User? user;
        private UserService userService;
        private UserHelper userHelper;
        private StagesHelper stagesHelper;

        public StagesController()
        {
            userService = new UserService();
            userHelper = new UserHelper();
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
        public IActionResult List()
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            var stages = stagesHelper.List("" + HttpContext.Session.GetString(Program.SessionContainerName));

            return View(stages);
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
        public IActionResult Create(Stage stage)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            stagesHelper.Save(stage, "" + HttpContext.Session.GetString(Program.SessionContainerName));

            return RedirectToAction("List", "Stages");
        }

        [HttpGet]
        public IActionResult Edit(string op)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            var stage = stagesHelper.Get(op);
            if (stage is null)
                return RedirectToAction("List", "Stages");

            return View(stage);
        }

        [HttpPost]
        public IActionResult Edit(Stage stage)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            stagesHelper.Save(stage, "" + HttpContext.Session.GetString(Program.SessionContainerName));

            return RedirectToAction("List", "Stages");
        }

        [HttpGet]
        public IActionResult Delete(string op)
        {
            if (user is null)
                return RedirectToAction("Login", "User");

            if (user.AccessLevel == 0)
                return RedirectToAction("Login", "User");

            stagesHelper.Delete(op);

            return RedirectToAction("List", "Stages");
        }
    }
}