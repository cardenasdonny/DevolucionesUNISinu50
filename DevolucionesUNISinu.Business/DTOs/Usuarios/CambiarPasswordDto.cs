using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Usuarios
{
    public class CambiarPasswordDto
    {
        public string UsuarioId { get; set; }
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual", Prompt = "Ingrese la contraseña actual", Description = "contraseña")]
        [StringLength(20, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 9)]
        public string ActualPassword { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña", Prompt = "Ingrese la nueva contraseña", Description = "contraseña")]
        public string NuevoPassword { get; set; }

        [Required(ErrorMessage = "Debe confirmar la nueva contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña", Prompt = "Confirmar contraseña", Description = "contraseña")]
        [Compare("NuevoPassword", ErrorMessage =
            "La nueva contraseña debe coincidir con la confirmación de contraseña")]
        public string ConfirmarPassword { get; set; }
    }
}
