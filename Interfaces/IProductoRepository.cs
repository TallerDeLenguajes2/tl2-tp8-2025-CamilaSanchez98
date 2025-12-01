using DistribuidoraInsumosMVC.Models;

namespace MVC.Interfaces
{
    public interface IProductoRepository
    {
        public bool CrearProducto(Producto producto);
        public bool ActualziarProducto(int idProducto, Producto productoModificado);
        public List<Producto> ListarProductos();
        public Producto GetProductoById(int idProducto);
        public bool EliminarProducto(int idProducto);
    }
}