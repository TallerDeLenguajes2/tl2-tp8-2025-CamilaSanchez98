using DistribuidoraInsumosMVC.Repositories;
using DistribuidoraInsumosMVC.Models;
using DistribuidoraInsumosMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;

namespace DistribuidoraInsumosMVC.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IAuthenticationService _authService;

        public ProductosController(IProductoRepository prodRepo, IAuthenticationService authService)
        {
            _productoRepository = prodRepo;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            List<Producto> productos = _productoRepository.ListarProductos();
            return View(productos);
        }

        [HttpGet]
        public IActionResult Details(int idProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var producto = _productoRepository.GetProductoById(idProducto);
            return (producto != null) ? View(producto) : NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            return View(new ProductoViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductoViewModel productoVM)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid) return View(productoVM);

            var nuevoProducto = new Producto
            {
                descripcion = productoVM.Descripcion,
                precio = (int)productoVM.Precio
            };

            bool creado = _productoRepository.CrearProducto(nuevoProducto);
            return (!creado) ? View(productoVM) : RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int idProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

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
        public IActionResult Update(int idProducto, ProductoViewModel productoVM)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid) return View(productoVM);
            if (idProducto != productoVM.IdProducto) return NotFound();

            var productoAeditar = new Producto
            {
                id = productoVM.IdProducto,
                descripcion = productoVM.Descripcion,
                precio = (int)productoVM.Precio
            };

            bool actualizado = _productoRepository.ActualziarProducto(idProducto, productoAeditar);
            return (!actualizado) ? View(productoAeditar) : RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int idProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            var producto = _productoRepository.GetProductoById(idProducto);
            return (producto != null) ? View(producto) : NotFound();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int idProducto)
        {
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            bool eliminado = _productoRepository.EliminarProducto(idProducto);
            return (!eliminado) ? View() : RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        // Método de chequeo de seguridad centralizado
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