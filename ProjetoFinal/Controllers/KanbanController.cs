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

        public KanbanController()
        {
            userService = new UserService();
            userHelper = new UserHelper();
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

            return View();
        }
    }
}