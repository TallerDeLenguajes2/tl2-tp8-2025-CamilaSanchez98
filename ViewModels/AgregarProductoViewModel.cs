using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraInsumosMVC.ViewModels
{
    public class AgregarProductoViewModel
    {
        [Required]
        public int IdPresupuesto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [Display(Name = "Producto a agregar")]
        public int IdProducto { get; set; } //viene de ser seleccionado en el dropdown

        [Required(ErrorMessage = "Debe ingresar una cantidad.")]
        [Display(Name = "Cantidad")]
        [Range(1,int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
        public int Cantidad { get; set; }

        // Lista para el dropdown del formulario
        public SelectList ListaProductos { get; set; }
    }
}