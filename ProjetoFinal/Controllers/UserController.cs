using ProjetoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjetoFinal.Controllers
{
    public class UserController : Controller
    {
        private User? user;
        private UserService userService;
        private UserHelper userHelper;

        public UserController()
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Pass))
            {
                string userSession = userHelper.AuthUser(model);
                HttpContext.Session.SetString(Program.SessionContainerName, userSession);
            }

            return RedirectToAction("List", "Kanban");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString(Program.SessionContainerName, "");
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Regist()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Regist(UserRegist model)
        {
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Pass) &&
                !string.IsNullOrWhiteSpace(model.ConfirmPass) && !string.IsNullOrWhiteSpace(model.Name))
            {
                string userSession = userHelper.RegistUser(model);
                HttpContext.Session.SetString(Program.SessionContainerName, userSession);
            }

            return RedirectToAction("List", "Kanban");
        }
    }
}