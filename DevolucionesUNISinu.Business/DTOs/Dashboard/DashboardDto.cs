using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int DevolucionesAbiertas { get; set; }
        public int DevolucionesCerradas { get; set; }
        public int DevolucionesTotales { get; set; }
        public float DevolucionesCerradasPorcentaje { get; set; }
    }
}
