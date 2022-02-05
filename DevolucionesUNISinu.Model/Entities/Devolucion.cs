using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class Devolucion
    {
        [Key]        
        public int DevolucionId { get; set; }      
        public string NumeroRadicado { get; set; }     
        public int Estado { get; set; }//1 abierto, 2 cerrado           
        public string NumeroMetodoConsignacion { get; set; }    
        public string ConceptoDevolucionOtro { get; set; }
        public decimal Valor { get; set; }
        public string Justificacion { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaRecepcion { get; set; }        
        public DateTime FechaRespuesta { get; set; }
        public string RutaArchivoRespuestaContabilidad { get; set; }
        public string RutaArchivoRespuestaTesoreria { get; set; }

        public int EstudianteId { get; set; }        
        public virtual Estudiante Estudiante { get; set; }

        public int BancoId { get; set; }
        public virtual Banco Banco { get; set; }

        public int MetodoConsignacionId { get; set; }
        public virtual MetodoConsignacion MetodoConsignacion { get; set; }

        public int ConceptoDevolucionId { get; set; }
        public virtual ConceptoDevolucion ConceptoDevolucion { get; set; }

        public virtual List<DevolucionDetalleArchivo> DevolucionDetalleArchivo { get; set; }

        //Usuario que responde la solicitud
        
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

    }
}
