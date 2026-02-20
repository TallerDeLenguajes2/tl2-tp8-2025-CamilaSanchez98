using System.ComponentModel.DataAnnotations;  // para validaciones....

namespace DistribuidoraInsumosMVC.ViewModels
{
    public class LoginViewModel
    {
        public string Username;
        public string Password;
        public string ErrorMessage;
    }
}