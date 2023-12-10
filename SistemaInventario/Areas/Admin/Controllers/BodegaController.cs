using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public BodegaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert (int? id)
        {
            Bodega bodega = new Bodega();
            if(id == null)
            {
                // crear una nueva bodega
                bodega.Estado = true;
                return View(bodega);
            }
            //Actualizamos Bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if(bodega == null)
            {
                return NotFound();
            }
            return View(bodega);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    TempData[DS.Exitosa] = "Bodega creada Exitosamente";
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                }
                {
					TempData[DS.Exitosa] = "Bodega actualizada Exitosamente";
					_unidadTrabajo.Bodega.Actuializar(bodega);
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
			TempData[DS.Error] = "Error al guardar Bodega";
			return View(bodega);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDb = await _unidadTrabajo.Bodega.Obtener(id);
            if(bodegaDb == null)
            {
                return Json(new {success = false, message = "Erro al borrar la Bodega"});
            } else
            {
                _unidadTrabajo.Bodega.Remover(bodegaDb);
                await _unidadTrabajo.Guardar();
                return Json(new { success = true, message = "Bodega borrada exitosamente" });
            }
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
            if(id == 0)
            {
                valor = lista.Any(i => i.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            } else
            {
				valor = lista.Any(i => i.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && i.Id != id);
			}
            if(valor)
            {
                return Json(new { data = true });
            }
			return Json(new { data = false });
		}
        #endregion
    }
}
