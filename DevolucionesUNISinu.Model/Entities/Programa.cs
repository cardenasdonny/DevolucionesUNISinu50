using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class Programa
    {
        [Key]
        public int ProgramaId { get; set; }
        public string Nombre { get; set; }
        public int Semestres { get; set; }     
        public bool Estado { get; set; }
        public int TipoProgramaId { get; set; }
        public int FacultadId { get; set; }
        public virtual Facultad Facultad { get; set; }
        public virtual TipoPrograma TipoPrograma { get; set; }
        public virtual List<Estudiante> Estudiantes { get; set; }

    }
}
