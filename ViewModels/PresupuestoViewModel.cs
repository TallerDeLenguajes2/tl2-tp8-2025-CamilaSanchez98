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
        [FechaValida]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }
    }

    public class FechaValida: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime fechaIngresada)
            {
                return fechaIngresada <= DateTime.Today;
            }
            return false;
        }
    }
}