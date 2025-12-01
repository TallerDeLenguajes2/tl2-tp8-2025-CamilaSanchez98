using DistribuidoraInsumosMVC.Repositories;
using DistribuidoraInsumosMVC.Models;
using DistribuidoraInsumosMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //para el SelectList

namespace DistribuidoraInsumosMVC.Controllers
{  
    
    public class PresupuestosController : Controller
    {
        private PresupuestoRepository _presupuestoRepository;
        private ProductoRepository _prodRepo;

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
        public IActionResult Update(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            if (presupuesto == null) return NotFound();

            var model = new PresupuestoViewModel
            {
                IdPresupuesto = idPresupuesto,
                NombreDestinatario = presupuesto.nombreDestinatario,
                FechaCreacion = presupuesto.fechaCreacion
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(int idPresupuesto, PresupuestoViewModel presupuestoVM)
        {
            if(!ModelState.IsValid) return View(presupuestoVM);
            if(idPresupuesto != presupuestoVM.IdPresupuesto) return NotFound();

                var objeto = new Presupuesto
                {
                    idPresupuesto = idPresupuesto,
                    nombreDestinatario = presupuestoVM.NombreDestinatario,
                    fechaCreacion = presupuestoVM.FechaCreacion
                };
            
            bool actualizado = _presupuestoRepository.ActualizarPresupuesto(idPresupuesto, objeto);
            return (!actualizado)? View(objeto): RedirectToAction(nameof(Index));
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
            return (!eliminado)? View():RedirectToAction(nameof(Index));
        }

        //AGREGAR PRODUCTO A PRESUPUESTON EXISTENTE
        [HttpGet]
        public IActionResult AddProduct(int idPresupuesto)
        {
            var pres = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            if (pres == null) return NotFound();

            var vm = new AgregarProductoViewModel
            {
                IdPresupuesto = idPresupuesto,
                ListaProductos = new SelectList(_prodRepo.ListarProductos(), "IdProducto", "Descripcion")
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddProduct(AgregarProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.ListaProductos =
                    new SelectList(_prodRepo.ListarProductos(), "id", "descripcion");
                return View(vm);
            }

            var prod = _prodRepo.GetProductoById(vm.IdProducto);
            if(prod==null) return NotFound();

            var detalle = new PresupuestoDetalle
            {
                producto = prod,
                cantidad = vm.Cantidad
            };

            //revisar si no debo corregir la logia del repositorio
            bool agregado = _presupuestoRepository.AgregarProducto(vm.IdPresupuesto, detalle);

            return (!agregado)? View(vm):RedirectToAction("Details", new { idPresupuesto = vm.IdPresupuesto });
        }
    }
}