using DistribuidoraInsumosMVC.Repositories;
using DistribuidoraInsumosMVC.Models;
using DistribuidoraInsumosMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Interfaces;

namespace DistribuidoraInsumosMVC.Controllers
{
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly IProductoRepository _prodRepo;
        private readonly IAuthenticationService _authService;

        public PresupuestosController(IPresupuestoRepository presRepo, IProductoRepository prodRepo, IAuthenticationService authService)
        {
            _presupuestoRepository = presRepo;
            _prodRepo = prodRepo;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (!_authService.HasAccessLevel("Administrador") && !_authService.HasAccessLevel("Cliente"))
                return RedirectToAction(nameof(AccesoDenegado));

            var presupuestos = _presupuestoRepository.ListarPresupuestos();
            return View(presupuestos);
        }

        [HttpGet]
        public IActionResult Details(int idPresupuesto)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (!_authService.HasAccessLevel("Administrador") && !_authService.HasAccessLevel("Cliente"))
                return RedirectToAction(nameof(AccesoDenegado));

            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            return (presupuesto != null) ? View(presupuesto) : NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            return View(new PresupuestoViewModel());
        }

        [HttpPost]
        public IActionResult Create(PresupuestoViewModel presupuestoVM)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid) return View(presupuestoVM);

            var nuevoPresupuesto = new Presupuesto
            {
                nombreDestinatario = presupuestoVM.NombreDestinatario,
                fechaCreacion = presupuestoVM.FechaCreacion
            };

            bool creado = _presupuestoRepository.CrearPresupuesto(nuevoPresupuesto);
            return (!creado) ? View(presupuestoVM) : RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var presupuesto = _presupuestoRepository.GetPresupuestoById(id);
            if (presupuesto == null) return NotFound();

            var model = new PresupuestoViewModel
            {
                IdPresupuesto = id,
                NombreDestinatario = presupuesto.nombreDestinatario,
                FechaCreacion = presupuesto.fechaCreacion
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int id, PresupuestoViewModel presupuestoVM)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            if (id != presupuestoVM.IdPresupuesto) return NotFound();
            if (!ModelState.IsValid) return View(presupuestoVM);

            var objeto = new Presupuesto
            {
                idPresupuesto = id,
                nombreDestinatario = presupuestoVM.NombreDestinatario,
                fechaCreacion = presupuestoVM.FechaCreacion
            };

            bool actualizado = _presupuestoRepository.ActualizarPresupuesto(id, objeto);
            return (!actualizado) ? View(presupuestoVM) : RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int idPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            return (presupuesto != null) ? View(presupuesto) : NotFound();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int idPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            _presupuestoRepository.EliminarPresupuesto(idPresupuesto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddProduct(int idPresupuesto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var pres = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            if (pres == null) return NotFound();

            var productos = _prodRepo.ListarProductos() ?? new List<Producto>();
            var vm = new AgregarProductoViewModel
            {
                IdPresupuesto = idPresupuesto,
                ListaProductos = new SelectList(productos, "id", "descripcion")
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddProduct(AgregarProductoViewModel vm)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid)
            {
                var productos = _prodRepo.ListarProductos() ?? new List<Producto>();
                vm.ListaProductos = new SelectList(productos, "id", "descripcion");
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

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        private IActionResult CheckAdminPermissions()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Index", "Login");

            if (!_authService.HasAccessLevel("Administrador"))
                return RedirectToAction(nameof(AccesoDenegado));

            return null;
        }
    }
}