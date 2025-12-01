using DistribuidoraInsumosMVC.Models;

namespace MVC.Interfaces
{
    public interface IPresupuestoRepository
    {
        public bool CrearPresupuesto(Presupuesto presupuesto);
        public List<Presupuesto> ListarPresupuestos();
        public Presupuesto GetPresupuestoById(int idPresupuesto);
        public bool AgregarProducto(int idPresupuesto, PresupuestoDetalle detalle);
        public bool ActualizarPresupuesto(int idPresupuesto,Presupuesto presupuesto);
        public bool EliminarPresupuesto(int idPresupuesto);
    }
}