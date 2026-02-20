using System.ComponentModel.DataAnnotations;  // para validaciones....

namespace DistribuidoraInsumosMVC.ViewModels
{
    public class LoginViewModel
    {
        public string Username {get;set;}
        public string Password {get;set;}
        public string ErrorMessage {get;set;}
    }
}