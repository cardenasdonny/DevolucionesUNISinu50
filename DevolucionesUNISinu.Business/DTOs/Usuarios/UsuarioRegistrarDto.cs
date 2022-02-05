using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Usuarios
{
    public class UsuarioRegistrarDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email", Order = -9,
        Prompt = "Ingrese el email", Description = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
      
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña", Order = -9,
        Prompt = "Ingrese la contraseña", Description = "Contraseña")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 9)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña", Order = -9,
        Prompt = "Confirme la contraseña", Description = "Confirme la contraseña")]
        [Compare("Password",
            ErrorMessage = "El Password y confirmar password debe coincidir")]
        public string ConfirmarPassword { get; set; }

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
