using DistribuidoraInsumosMVC.Repositories;
using DistribuidoraInsumosMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraInsumosMVC.Controllers
{  
    
    public class PresupuestosController : Controller
    {
        private PresupuestoRepository _presupuestoRepository;

        public PresupuestosController()
        {
            _presupuestoRepository = new PresupuestoRepository();
        }

        //LISTADOS Y DETALLES
        [HttpGet]
        public IActionResult Index()
        {
            List<Presupuesto> presupuestos = _presupuestoRepository.ListarPresupuestos();
            return View(presupuestos);
        }
        [HttpGet]
        public IActionResult Details(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            return (presupuesto != null) ? View(presupuesto) : NotFound();
        }

        //CREACION
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Presupuesto presupuesto)
        {
            bool creado = _presupuestoRepository.CrearPresupuesto(presupuesto);
            return (!creado)? View(presupuesto): RedirectToAction("Index");
        }

        //ACTUALIZACION
        [HttpGet]
        public IActionResult Update(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            return (presupuesto != null)? View(presupuesto):NotFound();
        }
        [HttpPost]
        public IActionResult Update(int idPresupuesto, Presupuesto presupuesto)
        {
            bool actualizado = _presupuestoRepository.ActualizarPresupuesto(idPresupuesto, presupuesto);
            return (!actualizado)? View(presupuesto): RedirectToAction("Index");
        }

        //ELIMINACION
        [HttpGet]
        public IActionResult Delete(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            return (presupuesto != null)? View(presupuesto):NotFound();
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int idPresupuesto)
        {
            bool eliminado =_presupuestoRepository.EliminarPresupuesto(idPresupuesto);
            return (!eliminado)? View():RedirectToAction("Index");
        }

        //AGREGAR PRODUCTO A PRESUPUESTON EXISTENTE
        [HttpGet]
        public IActionResult AddProduct(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);

            //para poder darle a la vista tambien la lista de productos
            ViewBag.Productos = new ProductoRepository().ListarProductos();
            return (presupuesto != null)? View(presupuesto):NotFound();
        }
        [HttpPost]
        public IActionResult AddProduct(int idPresupuesto,int idProducto, int cantidad)
        {
            ProductoRepository _proRep = new ProductoRepository();
            PresupuestoDetalle detalle = new PresupuestoDetalle
                                        {
                                            producto = _proRep.GetProductoById(idProducto),
                                            cantidad = cantidad
                                        };
            bool agregado = _presupuestoRepository.AgregarProducto(idPresupuesto, detalle);
            return (!agregado)? View(): RedirectToAction("Index");
        }
    }
}