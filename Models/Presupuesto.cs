namespace DistribuidoraInsumosMVC.Models
{
    public class Presupuesto
    {
        public int idPresupuesto { get; set; }
        public string nombreDestinatario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public List<PresupuestoDetalle> detalle { get; set; }

        public int MontoPresupuesto()
        {
            return 0;
        }
        public int MontoPresupuestoConIva() //considerar iva 21 
        {
            return 1;
        } 
        
        public int CantidadProductos() //contar total de productos (sumador de todas las cantidades del detalle)
        {
            return 2;
        }
    }
}   