using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Usuarios
{
    public class UsuarioEditarEmailDto
    {
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email", Order = -9,
        Prompt = "Ingrese el email", Description = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Confirmar email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Compare("Email",
            ErrorMessage = "El Password y confirmar password debe coincidir")]
        public string ConfirmarEmail { get; set; }
        [Display(Name = "Email actual")]
        public string EmailAnterior { get; set; }
    }
}
