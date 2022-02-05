using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevolucionesUNISinu.Model.Entities
{
    public class Estudiante
    {
        public int EstudianteId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public bool Estado { get; set; }
        public string Identificacion { get; set; }        
        public int Semestre { get; set; }
        public string Telefono { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }


        public int FacultadId { get; set; }
        public Facultad Facultad { get; set; }

        
        public int ProgramaId { get; set; }
        public Programa Programa { get; set; }


        public int TipoIdentificacionId { get; set; }
        public TipoIdentificacion TipoIdentificacion { get; set; }
        public virtual List<Devolucion> Devoluciones { get; set; }
    }
}
