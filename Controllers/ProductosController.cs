    using DistribuidoraInsumosMVC.Repositories;
    using DistribuidoraInsumosMVC.Models;
    using DistribuidoraInsumosMVC.ViewModels;
    using Microsoft.AspNetCore.Mvc;


    namespace DistribuidoraInsumosMVC.Controllers
    {
        public class ProductosController : Controller
        {
            private readonly ProductoRepository _productoRepository;

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
                return View(new ProductoViewModel());
            }
            [HttpPost]
            public IActionResult Create(ProductoViewModel productoVM)
            {
                if (!ModelState.IsValid) return View(productoVM);

                //mapeo manualmente de VM a Modelo de dominio
                var nuevoProducto = new Producto
                {
                    descripcion = productoVM.Descripcion,
                    precio = (int)productoVM.Precio
                };

                bool creado = _productoRepository.CrearProducto(nuevoProducto);
                return (!creado)? View(productoVM): RedirectToAction(nameof(Index));
            }

            //ACTUALIZACION
            [HttpGet]
            public IActionResult Update(int idProducto)
            {
                var producto = _productoRepository.GetProductoById(idProducto);
                if (producto == null) return NotFound();

                var productoVM = new ProductoViewModel
                {
                    IdProducto = producto.id,
                    Descripcion = producto.descripcion,
                    Precio = (decimal)producto.precio
                };
                return View(productoVM);
            }
            [HttpPost]
            public IActionResult Update(ProductoViewModel productoVM)
            {
                if(!ModelState.IsValid) return View(productoVM);

                var productoAeditar = new Producto
                {
                    id = productoVM.IdProducto,
                    descripcion = productoVM.Descripcion,
                    precio = (int)productoVM.Precio
                };

                bool actualizado = _productoRepository.ActualziarProducto(productoAeditar.id, productoAeditar);
                return (!actualizado)? View(productoVM): RedirectToAction(nameof(Index));
            }

            //ELIMINACION
            [HttpGet]
            public IActionResult Delete(int idProducto)
            {
                var producto = _productoRepository.GetProductoById(idProducto);
                return (producto != null)? View(producto):NotFound();
            }
            [HttpPost]
            public IActionResult DeleteConfirmed(int idProducto)
            {
                bool eliminado =_productoRepository.EliminarProducto(idProducto);
                return (!eliminado)? View(_productoRepository.GetProductoById(idProducto)):RedirectToAction("Index");
            }
        }
    }