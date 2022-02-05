using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Estudiantes;
using DevolucionesUNISinu.Business.DTOs.Facultades;
using DevolucionesUNISinu.Business.DTOs.Usuarios;
using DevolucionesUNISinu.Model.Entities;
using DevolucionesUNISinu.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Web.Controllers
{
    [Authorize]
    public class EstudiantesController : Controller
    {
        private readonly IEstudianteBusiness _estudianteBusiness;
        private readonly IFactultadBusiness _factultadBusiness;
        private readonly ITipoIdentificacionBusiness _tipoIdentificacionBusiness;
        private readonly IProgramaBusiness _programaBusiness;
        private readonly IUsuariosBusiness _usuariosBusiness;
        private readonly IRolBusiness _rolBusiness;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailBusiness _emailBusiness;

        public EstudiantesController(IEstudianteBusiness estudianteBusiness, IFactultadBusiness factultadBusiness, ITipoIdentificacionBusiness tipoIdentificacionBusiness, ITipoProgramaBusiness tipoProgramaBusiness, IProgramaBusiness programaBusiness, IUsuariosBusiness usuariosBusiness, IRolBusiness rolBusiness, UserManager<Usuario> userManager, IEmailBusiness emailBusiness)
        {
            _estudianteBusiness = estudianteBusiness;
            _factultadBusiness = factultadBusiness;
            _tipoIdentificacionBusiness = tipoIdentificacionBusiness;           
            _programaBusiness = programaBusiness;
            _usuariosBusiness = usuariosBusiness;
            _rolBusiness = rolBusiness;
            _userManager = userManager;
            _emailBusiness = emailBusiness;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Crear()
        {
            ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre");
            ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Crear(EstudianteRegistroDto estudianteRegistroDto)
        {
            if (ModelState.IsValid && estudianteRegistroDto.TipoIdentificacionId!=0)
            {

                //comprobamos que no exista el usuario
                var email = await _usuariosBusiness.ObtenerUsuarioDtoPorEmail(estudianteRegistroDto.Email);

                if (email == null)
                {
                    try
                    {
                        UsuarioRegistrarDto usuarioRegistrarDto = new()
                        {
                            Email = estudianteRegistroDto.Email,
                            Password = estudianteRegistroDto.Password,
                            Rol = "Estudiante",
                            Nombres = "Estudiante",
                            Apellidos = "Estudiante"
                        };
                        var usuarioId = await _usuariosBusiness.CrearUsuario(usuarioRegistrarDto);
                       
                        if (usuarioId != null)
                        {
                            if (usuarioId.Equals("ErrorPassword"))
                            {
                                TempData["Accion"] = "ErrorPassword";
                                TempData["Mensaje"] = "La contraseña debe contener mínimo 9 caracteres, se debe utilizar una contraseña que contenga mínimo un carácter en mayúsculas, mínimo un carácter en minúsculas y mínimo un carácter especial.";
                                return View(estudianteRegistroDto);
                            }
                            else
                            {
                                try
                                {
                                    var rol = await _rolBusiness.AsignarRolPorId(usuarioId, usuarioRegistrarDto.Rol);

                                    if (rol == true)
                                    {
                                        EstudianteDto estudianteDto = new()
                                        {
                                            Nombres = estudianteRegistroDto.Nombres,
                                            Apellidos = estudianteRegistroDto.Apellidos,
                                            TipoIdentificacionId = estudianteRegistroDto.TipoIdentificacionId,
                                            Identificacion = estudianteRegistroDto.Identificacion,
                                            Telefono = estudianteRegistroDto.Telefono.Trim(),
                                            FacultadId = estudianteRegistroDto.FacultadId,
                                            ProgramaId = estudianteRegistroDto.ProgramaId,
                                            Semestre = estudianteRegistroDto.Semestre,
                                            UsuarioId = usuarioId,
                                            Estado = true,

                                        };
                                        _estudianteBusiness.CrearEstudiante(estudianteDto);
                                        if (await _estudianteBusiness.GuardarCambios())
                                        {
                                            var usuario = await _usuariosBusiness.ObtenerUsuarioPorId(usuarioId);
                                            //se crea el token
                                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                                            //creamos un link
                                            var passwordresetLink = Url.Action("ConfirmarEmail", "Usuarios",
                                                new { email = usuario.Email, token = token }, Request.Scheme);
                                            //enviamos el correo
                                            var EnviarEmail = await _emailBusiness.EnviarEmail(usuario.Email, "Confirmación email - UniSinú", "Haga clic en el siguiente enlace para confirmar su email: " + passwordresetLink + " si no puede hacer clic, copie y pegue el enlace en su navegador");
                                            if (EnviarEmail)
                                            {
                                                TempData["Accion"] = "Registro";
                                                TempData["Mensaje"] = $"Se ha enviado un correo de confirmación a la dirección: {usuario.Email}, confirme el correo para iniciar sesión";
                                                return RedirectToAction("Login", "Usuarios");
                                            }
                                            else
                                            {
                                                //se debe hacer un rollback del estudiante creado
                                                TempData["Accion"] = "Error";
                                                TempData["Mensaje"] = "No se ha podido registrar el estudiante, existe un problema al enviar el correo";
                                                ModelState.AddModelError(string.Empty, "Error al registrar el estudiante.");
                                                return View(estudianteRegistroDto);

                                            }
                                        }
                                        else
                                        {
                                            //se debe hacer un rollback del estudiante creado
                                            TempData["Accion"] = "Error";
                                            TempData["Mensaje"] = "No se ha podido registrar el estudiante";
                                            ModelState.AddModelError(string.Empty, "Error al registrar el estudiante.");
                                            return View(estudianteRegistroDto);

                                        }

                                    }
                                    else
                                    {
                                        //se debe hacer un rollback del estudiante creado
                                        TempData["Accion"] = "Error";
                                        TempData["Mensaje"] = "No se ha podido registrar el estudiante";
                                        ModelState.AddModelError(string.Empty, "Error al registrar el estudiante.");
                                        return View(estudianteRegistroDto);
                                    }
                                }
                                catch (Exception)
                                {
                                    TempData["Accion"] = "Error";
                                    TempData["Mensaje"] = "No se ha podido registrar el estudiante";
                                    ModelState.AddModelError(string.Empty, "Error al registrar el estudiante.");
                                    return View(estudianteRegistroDto);
                                }

                            }

                            
                        }
                        else
                        {
                            TempData["Accion"] = "Password";
                            TempData["Mensaje"] = "La contraseña debe contener máximo 9 caracteres, se debe utilizar una contraseña que contenga mínimo un número, mínimo un carácter en mayúsculas y mínimo un carácter en minúsculas";
                            ModelState.AddModelError(string.Empty, "Error de registro");
                            ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre");
                            ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre");
                            return View(estudianteRegistroDto);
                        }                        

                    }
                    catch (Exception)
                    {

                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "No se ha podido registrar el estudiante";
                        ModelState.AddModelError(string.Empty, "Error al registrar el estudiante.");
                        ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre");
                        ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre");
                        return View(estudianteRegistroDto);
                    }

                }
                else
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "¡El estudiante ya existe!";
                    ModelState.AddModelError(string.Empty, "Ya existe un estudiante registrado con el correo.");
                    ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre");
                    ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre");
                    return View(estudianteRegistroDto);
                }             


            }//fin del modelstate
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "No se ha podido registrar el estudiante";
            ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre");
            ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre");
            ModelState.AddModelError(string.Empty, "Debe diligenciar todos los campos.");
            if(estudianteRegistroDto.TipoIdentificacionId==0)
                ModelState.AddModelError(string.Empty, "Debe seleccionar el tipo de documento.");
            return View(estudianteRegistroDto);
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var estudiante = await _estudianteBusiness.ObtenerEstudianteDetallePorId(id);
                    if (estudiante == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    return View(estudiante);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
        }



        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id != null)
            {
                try
                {
                    var estudiante = await _estudianteBusiness.ObtenerEstudiantePorId(id.Value);
                    if (estudiante == null)
                        return Json(new { isValid = false, error = "No se encuentra el registro" });
                    ViewData["ListaFacultades"] = new SelectList(await _factultadBusiness.ObtenerListaFacultadTodas(), "FacultadId", "Nombre", estudiante.FacultadId);
                    ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre", estudiante.TipoIdentificacionId);
                    ViewData["Programas"] = new SelectList(await _programaBusiness.ObtenerProgramasPorFacultad(estudiante.FacultadId), "ProgramaId", "Nombre", estudiante.ProgramaId);
                    ViewData["Semestres"] = new SelectList(await _programaBusiness.ObtenerSemestresPorPrograma(estudiante.ProgramaId), "Id", "Semestre", estudiante.Semestre);
                    return View(estudiante);
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });
                }

            }
            return Json(new { isValid = false, tipoError = "danger", error = "No se encuentra el registro" });

        }

        [HttpPost]
        public async Task<IActionResult> Editar(int? id, EstudianteDto estudianteDto)
        {
            if (id != estudianteDto.EstudianteId)
            {
                return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar la información" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _estudianteBusiness.Editar(estudianteDto);
                    var guardar = await _estudianteBusiness.GuardarCambios();
                    if (guardar)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                        return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar la información" });

                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "Error al actualizar la información" });
                }

            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", estudianteDto) });
        }

        //Los metodos que se encuentra abajo son los que llama el JS de la vista Crear


        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> ObtenerProgramasPorFacultad(int id)
        {
            List<ProgramaDto> listaPrograma = await _programaBusiness.ObtenerProgramasPorFacultad(id);
            listaPrograma.Insert(0, new ProgramaDto { ProgramaId = 0, Nombre = "--- Seleccione ---" });
            return Json(new SelectList(listaPrograma,"ProgramaId","Nombre"));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> ObtenerSemestresPorPrograma(int id)
        {
            List<SemestreDto> listaSemestres = await _programaBusiness.ObtenerSemestresPorPrograma(id);
            listaSemestres.Insert(0, new SemestreDto { Id = 0, Semestre = 0 });
            return Json(new SelectList(listaSemestres, "Id", "Semestre"));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> ObtenerFacultades()
        {
            var IEFacultades = await _factultadBusiness.ObtenerListaFacultadTodasEstado();
            var ListaFacultades = IEFacultades.ToList();
            ListaFacultades.Insert(0, new FacultadDto { FacultadId = 0, Nombre = "--- Seleccione ---" });
            return Json(new SelectList(ListaFacultades, "FacultadId", "Nombre"));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> ObtenerTiposIdentificacion()
        {
            ViewData["ListaTipoDocumento"] = new SelectList(await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos(), "TipoIdentificacionId", "Nombre");

            var IETiposIdentificacion = await _tipoIdentificacionBusiness.ObtenerListaTipoIdentificacionTodos();
            var ListaTiposIdentificacion = IETiposIdentificacion.ToList();
            ListaTiposIdentificacion.Insert(0, new TipoIdentificacionDto {TipoIdentificacionId = 0, Nombre = "--- Seleccione ---" });
            return Json(new SelectList(ListaTiposIdentificacion, "TipoIdentificacionId", "Nombre"));
        }
    }
}
