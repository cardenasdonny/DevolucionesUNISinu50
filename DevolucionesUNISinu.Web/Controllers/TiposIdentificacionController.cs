using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Estudiantes;
using DevolucionesUNISinu.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Web.Controllers
{
    [Authorize(Roles = "Administrador, Observador")]
    public class TiposIdentificacionController : Controller
    {
        private readonly ITipoIdentificacionBusiness _tipoIdentificacionBusiness;

        public TiposIdentificacionController(ITipoIdentificacionBusiness tipoIdentificacionBusiness)
        {
            _tipoIdentificacionBusiness = tipoIdentificacionBusiness;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Titulo"] = "Tipos de identificación";
            return View(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodosEstado());
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Crear()
        {
            return View();
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Crear(TipoIdentificacionDto tipoIdentificacionDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _tipoIdentificacionBusiness.CrearTipoIdentificacion(tipoIdentificacionDto);
                    var guardar = await _tipoIdentificacionBusiness.GuardarCambios();
                    if (guardar)
                        return Json(new { isValid = true, operacion = "crear" });
                    else
                        return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el registro" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el registro" });
                }
            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", tipoIdentificacionDto) });
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var buscar = await _tipoIdentificacionBusiness.ObtenerTipoIdentificacionPorId(id.Value);
                    if (buscar == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(buscar);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id != null)
            {
                try
                {
                    var buscar = await _tipoIdentificacionBusiness.ObtenerTipoIdentificacionPorId(id.Value);
                    if (buscar == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(buscar);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Editar(int? id, TipoIdentificacionDto tipoIdentificacionDto)
        {
            if (id != tipoIdentificacionDto.TipoIdentificacionId)
            {
                return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _tipoIdentificacionBusiness.EditarTipoIdentificacion(tipoIdentificacionDto);
                    var editar = await _tipoIdentificacionBusiness.GuardarCambios();
                    if (editar)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                        return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });

                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", tipoIdentificacionDto) });
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null)
            {
                return Json(new { isValid = false });
            }

            try
            {
                await _tipoIdentificacionBusiness.CambiarEstado(id.Value);
                var cambiarEstado = await _tipoIdentificacionBusiness.GuardarCambios();
                if (cambiarEstado)
                    return Json(new { isValid = true });
                else
                    return Json(new { isValid = false });
            }
            catch (Exception)
            {
                return Json(new { isValid = false });
            }
        }


    }
}
