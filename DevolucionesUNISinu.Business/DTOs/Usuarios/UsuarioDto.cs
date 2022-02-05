using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public string Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }   
        public string Rol { get; set; }
        public bool Estado { get; set; }
    }
}
