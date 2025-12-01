namespace DistribuidoraInsumosMVC.Models
{
    public class Presupuesto
    {
        private const double IVA = 0.21;
        public int idPresupuesto { get; set; }
        public string nombreDestinatario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public List<PresupuestoDetalle> detalle { get; set; }

        public double MontoPresupuesto()
        {
            double monto = 0;
            foreach (var item in detalle)
            {
                monto += item.producto.precio * item.cantidad;
            }
            return monto;
        }
        public double MontoPresupuestoConIva()
        {
            return MontoPresupuesto()*IVA;
        } 
        
        public int CantidadProductos()
        {
            int cant = 0;
            foreach (var item in detalle)
            {
                cant += item.cantidad;
            }
            return cant;
        }
    }
} 