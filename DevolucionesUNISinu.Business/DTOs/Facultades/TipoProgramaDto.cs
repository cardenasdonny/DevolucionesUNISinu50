using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Facultades
{
    public class TipoProgramaDto
    {
        [Required]     
        public int TipoProgramaId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }

    }
}
