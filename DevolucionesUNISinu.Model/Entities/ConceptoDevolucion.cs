using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class ConceptoDevolucion
    {
        [Key]
        public int ConceptoDevolucionId { get; set; }
        [Required]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string DocumentosRequeridos { get; set; }
    }
}
