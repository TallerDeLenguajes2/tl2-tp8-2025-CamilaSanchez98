using Microsoft.AspNetCore.Mvc;
using DistribuidoraInsumosMVC.ViewModels;
using DistribuidoraInsumosMVC.Interfaces;

namespace DistribuidoraInsumosMVC.Controllers
{

    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        // [HttpGet] Muestra la vista de login
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }
        // [HttpPost] Procesa el login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Debe ingresar usuario y contraseñaaa.";
                return View("Index", model);
            }
            if (_authenticationService.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = "Credenciales inválidass.";
            return View("Index", model);
        }

        // [HttpGet] Cierra sesión
        [HttpGet]
        public IActionResult Logout()
        {
            _authenticationService.Logout();
            return RedirectToAction("Index");
        }
    }

}