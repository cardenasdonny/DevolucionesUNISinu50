using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IEmailBusiness
    {
        Task<bool> EnviarEmail(string to, string subject, string content);
        Task<bool> EnviarEmailEstudiante(int devolucionId, string asunto, string contenido);
        Task<string> ObtenerEmailEstudiantePorDevolucionId(int? id);
       
    }
}
