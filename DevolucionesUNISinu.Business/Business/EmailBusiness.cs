using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Usuarios;
using DevolucionesUNISinu.Model.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace DevolucionesUNISinu.Business.Business
{
    public class EmailBusiness: IEmailBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public EmailBusiness(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;         
            _context = context;
        }
        public async Task<bool> EnviarEmail(string to, string asunto, string contenido)
        {
            var clienteCorreo = new SendGridClient(_configuration["Email:Key"]);
            var mensaje = new SendGridMessage
            {
                From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]),
                Subject = asunto,
                PlainTextContent = contenido,
                HtmlContent = contenido,               

            };
            mensaje.AddTo(to);
            mensaje.SetClickTracking(false, false);
            var respuesta = await clienteCorreo.SendEmailAsync(mensaje);
            var al = respuesta.StatusCode;
            return respuesta.IsSuccessStatusCode;

            //Opción en la que usamos smtp

            /*
            MailMessage mensaje = new();
            mensaje.To.Add(olvidePasswordDto.Email); //destinatario
            mensaje.Subject = "CrudEmpleados recuperar password";
            mensaje.Body = passwordresetLink;
            mensaje.IsBodyHtml = false;
            //mensaje.From = new MailAddress("pruebas@xofsystems.com","Notificaciones");
            mensaje.From = new MailAddress(_configuration["Mail"], "Notificaciones");
            SmtpClient smtpClient = new("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["Mail"], "Tempo123!");
            smtpClient.Send(mensaje);
            return View("OlvidePasswordConfirmacion");
            */
        }


        public async Task<string> ObtenerEmailEstudiantePorDevolucionId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var devolucion = await _context.Devoluciones.Include(x => x.Estudiante).Include(x => x.Estudiante.Usuario).FirstOrDefaultAsync(x => x.DevolucionId == id.Value);

            if (devolucion != null)                
                return devolucion.Estudiante.Usuario.Email;          

            else
                return null;

        }

        //incluye correo de administradores y observadores
        public async Task<List<string>> ObtenerListaCorreosAdmones()
        {
            var listado = await (from user in _context.Users
                                 join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                                 join role in _context.Roles on userRoles.RoleId equals role.Id
                                 where !role.Name.Equals("Estudiante")
                                 select (user.Email)
                                 )
                        .ToListAsync();
            return listado;
        }

        // se le envia un correo al alumno
        public async Task<bool> EnviarEmailEstudiante(int devolucionId, string asunto, string contenido)
        {
            if (devolucionId != 0 && asunto != null && contenido != null)
            {

                var emailEstudiante = await ObtenerEmailEstudiantePorDevolucionId(devolucionId);

                if (emailEstudiante != null)
                {
                    var clienteCorreo = new SendGridClient(_configuration["Email:Key"]);
                    var mensaje = new SendGridMessage
                    {
                        From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]),
                        Subject = asunto,
                        PlainTextContent = contenido,
                        HtmlContent = contenido,

                    };     
                    mensaje.AddTo(emailEstudiante);            
                    mensaje.SetClickTracking(false, false);
                    var respuesta = await clienteCorreo.SendEmailAsync(mensaje);
                    return respuesta.IsSuccessStatusCode;                  

                }
            }
            return false;
        }

        // se le envia un correo a los administradores - observadores y al alumno
        /*
        public async Task<bool> EnviarEmailEstudianteAdmones(int devolucionId, string asunto, string contenido)
        {
            if (devolucionId != 0 && asunto != null && contenido != null)
            {

                var emailEstudiante = await ObtenerEmailEstudiantePorDevolucionId(devolucionId);

                if (emailEstudiante != null)
                {
                    var clienteCorreo = new SendGridClient(_configuration["Email:Key"]);
                    var mensaje = new SendGridMessage
                    {
                        From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]),
                        Subject = asunto,
                        PlainTextContent = contenido,
                        HtmlContent = contenido,

                    };
                    var listaEmailsAdmones = await ObtenerListaCorreosAdmones();
                    List<EmailAddress> emailAddresses = new();

                    if (listaEmailsAdmones != null)
                    {
                        foreach (var email in listaEmailsAdmones)
                        {
                            EmailAddress emailAddress = new()
                            {
                                Email = email,
                                Name = _configuration["Email:FromName"]
                            };
                            emailAddresses.Add(emailAddress);                            
                        }
                        mensaje.AddTo(emailEstudiante);
                        mensaje.AddBccs(emailAddresses);
                        mensaje.SetClickTracking(false, false);
                        var respuesta = await clienteCorreo.SendEmailAsync(mensaje);
                        return respuesta.IsSuccessStatusCode;

                    }

                }
            }            
            return false;            
        }
        */
    }
}
