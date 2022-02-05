using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Usuarios
{
    public class EmailDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email", Order = -9,
       Prompt = "Ingrese el email", Description = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
