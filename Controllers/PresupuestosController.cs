using DistribuidoraInsumosMVC.Repositories;
using DistribuidoraInsumosMVC.Models;
using DistribuidoraInsumosMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //para el SelectList

namespace DistribuidoraInsumosMVC.Controllers
{

    public class PresupuestosController : Controller
    {
        private readonly PresupuestoRepository _presupuestoRepository;
        private readonly ProductoRepository _prodRepo;

        public PresupuestosController()
        {
            _presupuestoRepository = new PresupuestoRepository();
            _prodRepo = new ProductoRepository();
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
            return View(new PresupuestoViewModel());
        }
        [HttpPost]
        public IActionResult Create(PresupuestoViewModel presupuestoVM)
        {
            if (!ModelState.IsValid) return View(presupuestoVM);

            var nuevoPresupuesto = new Presupuesto
            {
                nombreDestinatario = presupuestoVM.NombreDestinatario,
                fechaCreacion = presupuestoVM.FechaCreacion
            };

            bool creado = _presupuestoRepository.CrearPresupuesto(nuevoPresupuesto);
            return (!creado)? View(nuevoPresupuesto): RedirectToAction(nameof(Index));
        }

        //ACTUALIZACION
        [HttpGet]
        public IActionResult Update(int id)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(id);
            if (presupuesto == null) return NotFound();

            var model = new PresupuestoViewModel
            {
                IdPresupuesto = id,
                NombreDestinatario = presupuesto.nombreDestinatario,
                FechaCreacion = presupuesto.fechaCreacion
            };
            return View("Update",model);
        }
        [HttpPost]
        public IActionResult Update(int id, PresupuestoViewModel presupuestoVM)
        {
            if(id != presupuestoVM.IdPresupuesto) return NotFound();
            if(!ModelState.IsValid) return View(presupuestoVM);

            var objeto = new Presupuesto
            {
                idPresupuesto = id,
                nombreDestinatario = presupuestoVM.NombreDestinatario,
                fechaCreacion = presupuestoVM.FechaCreacion
            };
            
            bool actualizado = _presupuestoRepository.ActualizarPresupuesto(id, objeto);
            return (!actualizado)? View(presupuestoVM): RedirectToAction(nameof(Index));
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
            _presupuestoRepository.EliminarPresupuesto(idPresupuesto); //podria manejar el bool qque devuleve..
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddProduct(int idPresupuesto)
        {
            var pres = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            if (pres == null) return NotFound();

            var productos = _prodRepo.ListarProductos() ?? new List<Producto>();

            var vm = new AgregarProductoViewModel
            {
                IdPresupuesto = idPresupuesto,
                ListaProductos = new SelectList(productos, "id", "descripcion")
            };

            return View(vm); //recuerda que el return View(vm) hace que la pagina se recargue con los datos del vm cargados en el form y se muestran  los errores de los <span>
        }
        [HttpPost]
        public IActionResult AddProduct(AgregarProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var productos = _prodRepo.ListarProductos() ?? new List<Producto>();
                vm.ListaProductos = new SelectList(productos, "id", "descripcion"); // id y descripcion Tiene que coincidir con las propiedades de la clase Producto.
                return View(vm);
            }

            var prod = _prodRepo.GetProductoById(vm.IdProducto);
            if (prod == null) return NotFound();

            var detalle = new PresupuestoDetalle
            {
                producto = prod,
                cantidad = vm.Cantidad
            };

            bool agregado = _presupuestoRepository.AgregarProducto(vm.IdPresupuesto, detalle);

            if (!agregado)
            {
                var productos = _prodRepo.ListarProductos() ?? new List<Producto>();
                vm.ListaProductos = new SelectList(productos, "id", "descripcion");
                return View(vm);
            }

            return RedirectToAction("Details", new { idPresupuesto = vm.IdPresupuesto });
        }
    }
}