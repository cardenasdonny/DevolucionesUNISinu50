using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Model.Entities
{
    public class Banco
    {
        [Key]
        public int BancoId { get; set; }    
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
