using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Facultades
{
    public class ProgramaDto
    {        
        public int ProgramaId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]        
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La facultad es requerida")]
        [DisplayName("Facultad")]        
        public int? FacultadId { get; set; }
        [DisplayName("Facultad")]
        public string NombreFacultad { get; set; }

        [Required(ErrorMessage = "Los semestres son requeridos")]
        [Range(1, 99, ErrorMessage = "Rango inválido")]
        public int? Semestres { get; set; }    
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Nivel académico requerido")]
        [DisplayName("Nivel académico")]
        public int? TipoProgramaId { get; set; }
        [DisplayName("Nivel académico")]
        public string NombreTipoPrograma { get; set; }
    }
}
