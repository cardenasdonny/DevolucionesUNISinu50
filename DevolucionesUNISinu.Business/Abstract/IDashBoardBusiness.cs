using DevolucionesUNISinu.Business.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IDashBoardBusiness
    {
        Task<DashboardDto> DashBoardAdmon();
        Task<DashboardDto> DashBoardEstudiante(int? estudianteId);
    }
}
