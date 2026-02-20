using Microsoft.AspNetCore.Mvc;
using DistribuidoraInsumosMVC.ViewModels;
using DistribuidoraInsumosMVC.Interfaces;

namespace DistribuidoraInsumosMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IAuthenticationService authenticationService, ILogger<LoginController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }
        // [HttpGet] Muestra la vista de login
        public IActionResult Index()
        {
            if (_authenticationService.IsAuthenticated()) return RedirectToAction("Index","Home");
            return View(new LoginViewModel());
        }
        // [HttpPost] Procesa el login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Debe ingresar usuario y contraseñaaa.";
                return View("Index",model);
            }
            if (_authenticationService.Login(model.Username, model.Password))
            {
                _logger.LogInformation("El usuario " + model.Username + " ingreso correctamente");
                return RedirectToAction("Index","Home");
            }

            _logger.LogWarning(@"intento de acceso invalido - Usuario: " + model.Username + " - Clave ingresada: " + model.Password);

            model.ErrorMessage = "Credenciales inválidass.";
            return View("Index", model);
        }

        // [HttpGet] Cierra sesión
        public IActionResult Logout()
        {
            _authenticationService.Logout();
            return RedirectToAction("Index","Home");
        }
    }

}