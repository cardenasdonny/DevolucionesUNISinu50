using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class Facultad
    {
        [Key]
        public int FacultadId { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public virtual List<Programa> Programas { get; set; }
    }
}
