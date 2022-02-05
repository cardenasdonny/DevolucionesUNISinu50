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
    [Authorize(Roles ="Administrador, Observador")]
    public class BancosController : Controller
    {
        private readonly IBancoBusiness _bancoBusiness;

        public BancosController(IBancoBusiness bancoBusiness)
        {
            _bancoBusiness = bancoBusiness;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Titulo"] = "Bancos";
            return View(await _bancoBusiness.ObtenerListaBancoTodosEstados());
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
        public async Task<IActionResult> Crear(BancoDto bancoDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bancoBusiness.CrearBanco(bancoDto);
                    var guardarFacultad = await _bancoBusiness.GuardarCambios();
                    if (guardarFacultad)
                        return Json(new { isValid = true, operacion = "crear" });
                    else
                        return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el registro" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el registro" });
                }


            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", bancoDto) });
        }


        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var bancoDto = await _bancoBusiness.ObtenerBancoPorId(id.Value);
                    if (bancoDto == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(bancoDto);
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
                    var facultadaDto = await _bancoBusiness.ObtenerBancoPorId(id.Value);
                    if (facultadaDto == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(facultadaDto);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });

        }
       
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? id, BancoDto bancoDto)
        {
            if (id != bancoDto.BancoId)
            {
                return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bancoBusiness.EditarBanco(bancoDto);
                    var guardarFacultad = await _bancoBusiness.GuardarCambios();
                    if (guardarFacultad)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                        return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });

                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", bancoDto) });
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
                await _bancoBusiness.CambiarEstado(id.Value);
                var cambiarEstado = await _bancoBusiness.GuardarCambios();
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
