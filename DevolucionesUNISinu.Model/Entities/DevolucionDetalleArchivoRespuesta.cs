using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class DevolucionDetalleArchivoRespuesta
    {
        [Key]
        public int DevolucionDetalleId { get; set; }

        [Required]
        [ForeignKey("Devolucion")]
        public int DevolucionId { get; set; }

        [Required]
        public string NombreArchivo { get; set; }

        public virtual Devolucion Devolucion { get; set; }
    }
}
