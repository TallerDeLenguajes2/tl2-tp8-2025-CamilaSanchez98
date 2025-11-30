    using DistribuidoraInsumosMVC.Repositories;
    using DistribuidoraInsumosMVC.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace DistribuidoraInsumosMVC.Controllers
    {  
        
        public class ProductosController : Controller
        {
            private ProductoRepository _productoRepository;

            public ProductosController()
            {
                _productoRepository = new ProductoRepository();
            }

            //LISTADOS Y DETALLES
            [HttpGet]
            public IActionResult Index()
            {
                List<Producto> productos = _productoRepository.ListarProductos();
                return View(productos);
            }
            [HttpGet]
            public IActionResult Details(int idProducto)
            {
                var producto = _productoRepository.GetProductoById(idProducto);
                return (producto != null) ? View(producto) : NotFound();
            }

            //CREACION
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public IActionResult Create(Producto producto)
            {
                bool creado = _productoRepository.CrearProducto(producto);
                return (!creado)? View(producto): RedirectToAction("Index");
            }

            //ACTUALIZACION
            [HttpGet]
            public IActionResult Update(int idProducto)
            {
                var producto = _productoRepository.GetProductoById(idProducto);
                return (producto != null)? View(producto):NotFound();
            }
            [HttpPost]
            public IActionResult Update(int idProducto, Producto producto)
            {
                bool actualizado = _productoRepository.ActualziarProducto(idProducto, producto);
                return (!actualizado)? View(producto): RedirectToAction("Index");
            }

            //ELIMINACION
            [HttpGet]
            public IActionResult Delete(int idProducto)
            {
                var producto = _productoRepository.GetProductoById(idProducto);
                return (producto == null)? View(producto):NotFound();
            }
            [HttpPost]
            public IActionResult DeleteConfirmed(int idProducto)
            {
                bool eliminado =_productoRepository.EliminarProducto(idProducto);
                return (!eliminado)? View():RedirectToAction("Index");
            }
        }
    }