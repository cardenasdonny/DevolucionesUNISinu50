using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class Usuario : IdentityUser
    {
        public bool Estado { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }
}
