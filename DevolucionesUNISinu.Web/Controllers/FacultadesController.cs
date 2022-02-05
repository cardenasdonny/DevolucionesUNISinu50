using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Facultades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevolucionesUNISinu.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace DevolucionesUNISinu.Web.Controllers
{
    //[Authorize(Roles = "Administrador, Observador")]
    public class FacultadesController : Controller
    {
        private readonly IFactultadBusiness _factultadBusiness;

        public FacultadesController(IFactultadBusiness factultadBusiness)
        {            
            _factultadBusiness = factultadBusiness;
        }
        public async Task<IActionResult> Index()
        {            
            ViewData["Titulo"] = "Facultades";
            return View(await _factultadBusiness.ObtenerListaFacultadTodasEstado());
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [NoDirectAccess]
        public IActionResult CrearFacultad()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearFacultad(FacultadDto facultadDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _factultadBusiness.CrearFacultad(facultadDto);
                    var guardarFacultad = await _factultadBusiness.GuardarCambios();
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
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "CrearFacultad", facultadDto) });
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var facultadDto = await _factultadBusiness.ObtenerFacultadPorId(id.Value);
                    if (facultadDto == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(facultadDto);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [NoDirectAccess]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id != null)
            {
                try
                {
                    var facultadaDto = await _factultadBusiness.ObtenerFacultadPorId(id.Value);
                    if (facultadaDto == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(facultadaDto);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }
                
            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro"});

        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? id, FacultadDto facultadDto)
        {
            if (id != facultadDto.FacultadId)
            {
                return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar el registro" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _factultadBusiness.EditarFacultad(facultadDto);
                    var guardarFacultad = await _factultadBusiness.GuardarCambios();
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
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", facultadDto) });
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [NoDirectAccess]
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null)
            {
                return Json(new { isValid = false });
            }

            try
            {
                await _factultadBusiness.CambiarEstado(id.Value);
                var CambiarEstadoFacultad = await _factultadBusiness.GuardarCambios();
                if (CambiarEstadoFacultad)
                    return Json(new { isValid = true });
                else
                    return Json(new { isValid = false });
            }
            catch (Exception)
            {
                return Json(new { isValid = false });
            }
            

        }


        /*
        public async Task<IActionResult> GenerarFacultades()
        {
            for (int i = 1; i < 200; i++)
            {
                FacultadDto FacultadDto = new()
                {
                    Estado = true,
                    Nombre = "Facultad prueba # " + i,
                };
                _factultadBusiness.GuardarFacultad(FacultadDto);
                await _factultadBusiness.GuardarCambios();
            }
            return Ok();
        }
        */
    }
}
