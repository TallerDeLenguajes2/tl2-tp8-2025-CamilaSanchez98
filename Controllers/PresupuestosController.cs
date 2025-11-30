
        
                //Peticiones
        
        // [HttpGet]
        // public ActionResult GetPresupuestoById(int idPresupuesto)
        // {
        //     var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
        //     return (presupuesto != null) ? Ok(presupuesto) : NotFound("Presupuesto no ENCONTRADO.");
        // }
        // [HttpPost]
        // public ActionResult CrearPresupuesto([FromBody] Presupuesto presupuesto)
        // {
        //     bool creado = _presupuestoRepository.CrearPresupuesto(presupuesto);
        //     return (creado)? Created(): BadRequest("Presupuesto no creado");
        // }
        
        // [HttpPost]
        // public ActionResult AgregarProducto(int idPresupuesto, [FromBody] PresupuestoDetalle detalle)
        // {
        //     bool agregado = _presupuestoRepository.AgregarProducto(idPresupuesto,detalle);
        //     return (agregado)? Ok():BadRequest("Producto NO agregado...error");
        // }
        // [HttpDelete]
        // public ActionResult EliminarPresupuesto(int idPresupuesto)
        // {
        //     bool eliminado =_presupuestoRepository.EliminarPresupuesto(idPresupuesto);

        //     return (eliminado)? NoContent() : BadRequest("Presupuesto NO eliminado...");
        // }

using DistribuidoraInsumosMVC.Repositories;
using DistribuidoraInsumosMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraInsumosMVC.Controllers
{  
    
    public class PresupuestosController : Controller
    {
        private PresupuestoRepository _presupuestoRepository;
        private readonly ILogger<PresupuestosController> _logger;

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
            return (presupuesto == null)? View(presupuesto):NotFound();
        }
        [HttpPost]
        public IActionResult Update(int idPresupuesto, Presupuesto presupuesto)
        {
            bool actualizado = _presupuestoRepository.ActualziarPresupuesto(idPresupuesto, presupuesto);
            return (!actualizado)? View(presupuesto): RedirectToAction("Index");
        }

        //ELIMINACION
        [HttpGet]
        public IActionResult Delete(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetPresupuestoById(idPresupuesto);
            return (presupuesto == null)? View(presupuesto):NotFound();
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int idPresupuesto)
        {
            bool eliminado =_presupuestoRepository.EliminarPresupuesto(idPresupuesto);
            return (!eliminado)? View():RedirectToAction("Index");
        }
    }
}