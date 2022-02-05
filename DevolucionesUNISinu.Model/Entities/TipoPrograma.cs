using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class TipoPrograma
    {
        [Key]
        public int TipoProgramaId { get; set; }
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }
        public virtual List<Programa> Programas { get; set; }
    }
}
