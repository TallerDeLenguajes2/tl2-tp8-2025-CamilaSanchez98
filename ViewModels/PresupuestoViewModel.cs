using System.ComponentModel.DataAnnotations;

namespace DistribuidoraInsumosMVC.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto;

        [Display(Name = "Nombre o correo electinico del destinatario")]
        [Required(ErrorMessage = "El nombre del destinatario es obligatorio.")]
        //[EmailAddress(ErrorMessage = "El formato e-mail ingresado no es valido")]
        public string NombreDestinatario { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de creación.")]
        //validacion de fecha no futura falta agregar...
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }
    }
}