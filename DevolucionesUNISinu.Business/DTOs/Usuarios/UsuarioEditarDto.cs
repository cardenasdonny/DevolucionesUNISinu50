using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Usuarios
{
    public class UsuarioEditarDto
    {
      
        public string UsuarioId { get; set; }
        
        public string Rol { get; set; }
        [Required(ErrorMessage = "Los nombres son requeridos")]
        [StringLength(30, ErrorMessage = "Los nombres deben contener entre 3 y 30 caracteres", MinimumLength = 3)]
        [Display(Name = "Nombres", Order = -9,
        Prompt = "Ingrese los nombres", Description = "Nombres")]
        public string Nombres { get; set; }

        

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [StringLength(30, ErrorMessage = "Los apellidos deben contener entre 3 y 30 caracteres", MinimumLength = 3)]
        [Display(Name = "Apellidos", Order = -9,
        Prompt = "Ingrese los apellidos", Description = "Apellidos")]
        public string Apellidos { get; set; }
    }
}
