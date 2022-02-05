using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Devoluciones;
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
    public class MetodosConsignacionController : Controller
    {
        private readonly IMetodoConsignacionBusiness _metodoConsignacionBusiness;

        public MetodosConsignacionController(IMetodoConsignacionBusiness  metodoConsignacionBusiness)
        {
            _metodoConsignacionBusiness = metodoConsignacionBusiness;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Titulo"] = "Métodos de consignación";
            return View(await _metodoConsignacionBusiness.ObtenerListaMetodoConsignacionTodosEstado());
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [NoDirectAccess]
        public IActionResult Crear()
        {
            return View();
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Crear(MetodoConsignacionDto metodoConsignacionDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _metodoConsignacionBusiness.CrearMetodoConsignacion(metodoConsignacionDto);
                    var guardar = await _metodoConsignacionBusiness.GuardarCambios();
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
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", metodoConsignacionDto) });
        }


        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var detalle = await _metodoConsignacionBusiness.ObtenerMetodoConsignacionPorId(id.Value);
                    if (detalle == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(detalle);
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
                    var editar = await _metodoConsignacionBusiness.ObtenerMetodoConsignacionPorId(id.Value);
                    if (editar == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(editar);
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
        public async Task<IActionResult> Editar(int? id, MetodoConsignacionDto metodoConsignacionDto)
        {
            if (id != metodoConsignacionDto.MetodoConsignacionId)
            {
                return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _metodoConsignacionBusiness.EditarMetodoConsignacion(metodoConsignacionDto);
                    var guardar = await _metodoConsignacionBusiness.GuardarCambios();
                    if (guardar)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                        return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });

                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", metodoConsignacionDto) });
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
                await _metodoConsignacionBusiness.CambiarEstado(id.Value);
                var cambiarEstado = await _metodoConsignacionBusiness.GuardarCambios();
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
