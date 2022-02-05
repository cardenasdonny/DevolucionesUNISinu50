using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class TipoIdentificacion
    {
        [Key]
        public int TipoIdentificacionId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }
        public virtual List<Estudiante> Estudiante { get; set; }
    }
}
