using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Devoluciones;
using DevolucionesUNISinu.Model.Entities;
using DevolucionesUNISinu.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Web.Controllers
{
    [Authorize]
    public class DevolucionesController : Controller
    {
        
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IDevolucionBusiness _devolucionBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConceptoDevolucionBusiness _conceptoDevolucionBusiness;
        private readonly IMetodoConsignacionBusiness _metodoConsignacionBusiness;
        private readonly IBancoBusiness _bancoBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IUsuariosBusiness _usuariosBusiness;
        private readonly UserManager<Usuario> _userManager;

        public DevolucionesController(SignInManager<Usuario> signInManager, IDevolucionBusiness devolucionBusiness, IWebHostEnvironment webHostEnvironment, IConceptoDevolucionBusiness conceptoDevolucionBusiness, IMetodoConsignacionBusiness metodoConsignacionBusiness, IBancoBusiness bancoBusiness, IEmailBusiness emailBusiness, IUsuariosBusiness usuariosBusiness, UserManager<Usuario> userManager)
        {            
            _signInManager = signInManager;
            _devolucionBusiness = devolucionBusiness;
            _webHostEnvironment = webHostEnvironment;
            _conceptoDevolucionBusiness = conceptoDevolucionBusiness;
            _metodoConsignacionBusiness = metodoConsignacionBusiness;
            _bancoBusiness = bancoBusiness;
            _emailBusiness = emailBusiness;
            _usuariosBusiness = usuariosBusiness;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string estado) //Traemos del menú el estado a consultar (0 Todas, 1 abiertas, 2 cerradas)
        {
            if (estado != null)
            {
                ViewData["estado"]=estado;
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var roles = await _userManager.GetRolesAsync(user);
                var rol = roles.FirstOrDefault();

                if (estado == "0")
                    ViewBag.Titulo = "Devoluciones - Todas";
                else if (estado == "1")
                    ViewBag.Titulo = "Devoluciones - Abiertas";
                else if (estado == "4")
                    ViewBag.Titulo = "Devoluciones - Cerradas";
                else
                    ViewBag.Titulo = "Devoluciones - Rechazadas";

                               

                if (_signInManager.IsSignedIn(User) && User.IsInRole("Estudiante"))
                {
                    /* CLAIMS
                    var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                    var usuarioNombre = User.FindFirstValue(ClaimTypes.Name);
                    */                   
                    
                    var listaDevoluciones = await _devolucionBusiness.ObtenerListaEncabezadoDevoluciones(int.Parse(estado), int.Parse(User.FindFirst("EstudianteId").Value), rol);
                    return View(listaDevoluciones);

                }
                else 
                //else if (_signInManager.IsSignedIn(User) && User.IsInRole("Administrador"))
                {                    
                    var listaDevoluciones = await _devolucionBusiness.ObtenerListaEncabezadoDevoluciones(int.Parse(estado), 0, rol);
                    return View(listaDevoluciones);

                }

            }
            else
                return NotFound();
        }
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Crear()
        {
            //var dias = HolidayUtil.CountBusinessDays(DateTime.Now, Convert.ToDateTime("2021-08-16"));

            ViewData["ListaConceptosDevolucion"] = new SelectList(await _conceptoDevolucionBusiness.ObtenerListaConceptoDevolucionTodos(), "ConceptoDevolucionId", "Nombre");
            ViewData["ListaBancos"] = new SelectList(await _bancoBusiness.ObtenerListaBancoTodos(), "BancoId", "Nombre");
            ViewData["ListaMetodosConsignacion"] = new SelectList(await _metodoConsignacionBusiness.ObtenerListaMetodoConsignacionTodos(), "MetodoConsignacionId", "Nombre");
            ViewBag.Titulo = "Solicitar devolución";
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Crear(DevolucionCrearDto devolucionDto)
        {
            
            if (ModelState.IsValid && devolucionDto.Files!=null)
            {
                //ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre");
                ViewBag.Titulo = "Solicitar devolución";

                if (_signInManager.IsSignedIn(User) && User.IsInRole("Estudiante"))
                {
                    try
                    {
                        //se crea la devolución
                        devolucionDto.NumeroRadicado = DateTime.Now.ToString("yyyymmssfff");
                        var devolucionId = await _devolucionBusiness.Crear(devolucionDto, int.Parse(User.FindFirst("EstudianteId").Value));                        
                        if (devolucionId != null)
                        {
                            //se crean los archivos
                            string uniqueFileName;
                            if (devolucionDto.Files != null && devolucionDto.Files.Count > 0)
                            {
                                foreach (IFormFile archivo in devolucionDto.Files)
                                {                                    
                                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "soportes_e");
                                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + archivo.FileName;
                                    uniqueFileName = DateTime.Now.ToString("yyyymmssfff") + "_" + (archivo.FileName).Trim(); //otra opción
                                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                    // Use CopyTo() method provided by IFormFile interface to
                                    // copy the file to wwwroot/images folder
                                    // archivo.CopyTo(new FileStream(filePath, FileMode.Create));
                                    using (var fileStream = new FileStream(filePath, FileMode.Create))//Guardar imagen
                                    {
                                        await archivo.CopyToAsync(fileStream);
                                    }
                                    _devolucionBusiness.CrearDevolucionDetalleArchivo(devolucionId.Value ,uniqueFileName);

                                }
                                if (await _devolucionBusiness.GuardarCambios())
                                {
                                    //se envia corre al estudiante y a los administradores
                                    await _emailBusiness.EnviarEmailEstudiante(devolucionId.Value, $"Solicitud # {devolucionDto.NumeroRadicado}", $"Se radicó la solicitud # {devolucionDto.NumeroRadicado} exitosamente");
                                    return Json(new { mensaje = $"Solicitud # {devolucionDto.NumeroRadicado} radicada con éxito", operacion = "crear" });
                                }
                                else
                                {
                                    return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al crear la solicitud" });
                                    //rollback

                                }
                            }                            
                        }
                        else
                        {
                            return Json(new {tipoError = "error", error = "Ocurrió un error interno al crear la solicitud" });
                            //rollback                            
                            //se registra el error
                        }

                    }
                    catch (Exception)
                    {
                        return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al crear la solicitud" });
                    }

                }
                else
                {
                    return Json(new { isValid = false, tipoError = "error-sesion", error = "Debe volver a iniciar sesión" });                  
                }                           
            }
            ViewData["ListaConceptosDevolucion"] = new SelectList(await _conceptoDevolucionBusiness.ObtenerListaConceptoDevolucionTodos(), "ConceptoDevolucionId", "Nombre");
            ViewData["ListaBancos"] = new SelectList(await _bancoBusiness.ObtenerListaBancoTodos(), "BancoId", "Nombre");
            ViewData["ListaMetodosConsignacion"] = new SelectList(await _metodoConsignacionBusiness.ObtenerListaMetodoConsignacionTodos(), "MetodoConsignacionId", "Nombre");
            ViewBag.Titulo = "Solicitar devolución";
            return Json(new { respuesta = "warning", mensaje = "Debe llenar los campos obligatorios"});
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var devolucionDto = await _devolucionBusiness.ObtenerDevolucionPorId(id.Value, User.Identity.Name);
                    if (devolucionDto != null)                        
                        return View(devolucionDto);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
        }

        public async Task<IActionResult> DescargarSoporte (string ruta)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, ruta);

            var memory = new MemoryStream();
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                await fileStream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(filePath);

            return File(memory, contentType, fileName);

        }

        [HttpGet]
        [Authorize(Roles = "Administrador, ApoyoFinanciero, Contabilidad, Tesoreria")]
        [NoDirectAccess]
        public async Task<IActionResult> Responder(int? id, int tipoRespuesta)
        {
            
            if (id != null)
            {
                try
                {
                    
                    var devolucionDto = await _devolucionBusiness.ObtenerDevolucionPorId(id.Value, User.Identity.Name);
                    if (devolucionDto != null)
                    {
                        ViewBag.Titulo = "Responder devolución radicado # " + devolucionDto.NumeroRadicado;
                        devolucionDto.tipoRespuesta = tipoRespuesta;
                        return View(devolucionDto);
                    }
                    else
                    {
                        //return NotFound();
                        return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                    }
                    
                }
                catch (Exception)
                {
                    //return StatusCode(500);                   
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador, ApoyoFinanciero, Contabilidad, Tesoreria")]
        public async Task<IActionResult> Responder(DevolucionDto devolucionDto)
        {
           
            if (_signInManager.IsSignedIn(User) && (!User.IsInRole("Estudiante")|| !User.IsInRole("Observador")))
            {
                int estado = 0;
                int sw=0;
                if (ModelState.IsValid)
                {
                    if (devolucionDto.tipoRespuesta == 1) // avanzar proceso
                    {
                    
                        if (devolucionDto.Estado == 1)
                            estado = 2;
                        if (devolucionDto.Estado == 2)
                            estado = 3;
                        if (devolucionDto.Estado == 3)
                            estado = 4;
                    }
                    else
                    {
                        if (devolucionDto.Estado == 1)
                            estado = 5;
                        if (devolucionDto.Estado == 2)
                            estado = 1;
                        if (devolucionDto.Estado == 3)
                            estado = 2;
                    }

                    devolucionDto.Estado = estado;

                    

                    if (User.IsInRole("Contabilidad"))
                    {
                        if (devolucionDto.File == null && devolucionDto.RutaArchivoRespuestaContabilidad==null)
                        {
                            return Json(new { isValid = false, tipoError = "warning", error = "Debe adjuntar la nota contable" });
                        }
                        else

                        if (devolucionDto.File != null)
                        {
                            //se crea el archivo
                            string uniqueFileName;
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "soportes_r");
                            uniqueFileName = DateTime.Now.ToString("yyyymmssfff") + "_" + (devolucionDto.File.FileName).Trim(); //otra opción
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))//Guardar archivo
                            {
                                await devolucionDto.File.CopyToAsync(fileStream);
                            }
                            devolucionDto.RutaArchivoRespuestaContabilidad = uniqueFileName;

                        }  
                        
                        

                    }
                    else if(User.IsInRole("Tesoreria"))
                    {
                        if (devolucionDto.File == null && devolucionDto.RutaArchivoRespuestaTesoreria == null)
                        {
                            return Json(new { isValid = false, tipoError = "warning", error = "Debe adjuntar el soporte de egreso" });
                        }
                        else

                       if (devolucionDto.File != null)
                        {
                            //se crea el archivo
                            string uniqueFileName;
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "soportes_r");
                            uniqueFileName = DateTime.Now.ToString("yyyymmssfff") + "_" + (devolucionDto.File.FileName).Trim(); //otra opción
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))//Guardar archivo
                            {
                                await devolucionDto.File.CopyToAsync(fileStream);
                            }
                            devolucionDto.RutaArchivoRespuestaTesoreria = uniqueFileName;

                        }

                    }                  
                    
                    
                    try
                    {

                        //se actualiza la devolución con la respuesta
                        
                        if(devolucionDto.Estado==3)
                            devolucionDto.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (await _devolucionBusiness.Responder(devolucionDto, User.FindFirstValue(ClaimTypes.NameIdentifier)))
                        {
                            if (await _devolucionBusiness.GuardarCambios())
                            {
                                if (devolucionDto.Estado == 3)
                                    await _emailBusiness.EnviarEmailEstudiante(devolucionDto.DevolucionId, $"Respuesta solicitud # {devolucionDto.NumeroRadicado}", $"La solicitud # {devolucionDto.NumeroRadicado} se ha cerrado, lo invitamos a consultar la respuesta");
                                if (devolucionDto.Estado == 5)
                                    //se envia corre al estudiante y a los administradores
                                    await _emailBusiness.EnviarEmailEstudiante(devolucionDto.DevolucionId, $"Respuesta solicitud # {devolucionDto.NumeroRadicado}", $"La solicitud # {devolucionDto.NumeroRadicado} ha sido rechazada, lo invitamos a consultar la respuesta");
                                return Json(new { isValid = true, operacion = "crear" });
                            }
                        }

                    }
                    catch (Exception)
                    {

                        return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al crear la respuesta" });
                    }

                }
                else
                {
                    return Json(new { isValid = false, tipoError = "warning", error = "Debe llenar los campos obligatorios" });
                }


                
                /*
                if (_signInManager.IsSignedIn(User) && User.IsInRole("Administrador, ApoyoLogistico, Contabilidad, Tesoreria"))
                {
                    if (ModelState.IsValid && devolucionDto.Files != null)
                    {
                        if (devolucionDto.Files != null && devolucionDto.Files.Count >= 1 && devolucionDto.Files.Count <= 10)
                        {
                            try
                            {
                                //se actualiza la devolución con la respuesta
                                if (await _devolucionBusiness.Responder(devolucionDto, User.FindFirstValue(ClaimTypes.NameIdentifier)))
                                {
                                    if (await _devolucionBusiness.GuardarCambios())
                                    {                                


                                        //se crean los archivos
                                        string uniqueFileName;                                    
                                        foreach (IFormFile archivo in devolucionDto.Files)
                                        {
                                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "soportes_r");
                                            uniqueFileName = DateTime.Now.ToString("yyyymmssfff") + "_" + (archivo.FileName).Trim(); //otra opción
                                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                            using (var fileStream = new FileStream(filePath, FileMode.Create))//Guardar imagen
                                            {
                                                await archivo.CopyToAsync(fileStream);
                                            }
                                            _devolucionBusiness.CrearDevolucionDetalleRespuesta(devolucionDto.DevolucionId, uniqueFileName);

                                        }
                                        if (await _devolucionBusiness.GuardarCambios())
                                        {
                                            //se envia corre al estudiante y a los administradores
                                            await _emailBusiness.EnviarEmailEstudiante(devolucionDto.DevolucionId, $"Respuesta solicitud # {devolucionDto.NumeroRadicado}", $"La solicitud # {devolucionDto.NumeroRadicado} se ha cerrado, lo invitamos a consultar la respuesta");
                                            return Json(new { isValid = true, operacion = "crear" });

                                        }
                                        else
                                        {
                                            return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al guardar los soportes" });
                                            //rollback
                                        }  



                                    }
                                    else
                                    {
                                        return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al crear la respuesta" });
                                    }
                                }
                                else
                                {
                                    return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al crear la respuesta" });
                                }

                            }
                            catch (Exception)
                            {

                                return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno al crear la respuesta" });
                            }

                        }//fin si devolucionDto.Files.Count>=1 && devolucionDto.Files.Count <= 10
                        else
                        {
                            return Json(new { isValid = false, tipoError = "warning", error = "Debe adjuntar mínimo un soporte" });

                        }
                    }//fin modelstate
                    else
                    {
                        return Json(new { isValid = false, tipoError = "warning", error = "Debe llenar los campos obligatorios" });

                    }
                }
                else
                {
                    return Json(new { isValid = false, tipoError = "error-sesion", error = "Debe volver a iniciar sesión" });                
                }

                 */
            }else
                return Json(new { isValid = false, tipoError = "error-sesion", error = "Debe volver a iniciar sesión" });

            return Json(new { isValid = false, tipoError = "danger", error = "Ocurrió un error interno" });

        }
        //temporal
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Generar()
        {
            await _devolucionBusiness.Generar();
            return Ok();
        }
    }    
}
