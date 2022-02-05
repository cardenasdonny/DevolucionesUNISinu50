using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevolucionesUNISinu.Business.DTOs.Facultades;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IProgramaBusiness
    {
        Task<IEnumerable<ProgramaDto>> ObtenerListaProgramaTodos();
        Task<ProgramaDto> ObtenerProgramaPorId(int? id);
        Task<List<ProgramaDto>> ObtenerProgramasPorFacultad(int? id);
        Task<List<SemestreDto>> ObtenerSemestresPorPrograma(int? id);
        void CrearPrograma(ProgramaDto programaDto);
        void EditarPrograma(ProgramaDto programaDto);
        Task CambiarEstado(int id);
        Task<bool> GuardarCambios();
        Task<IEnumerable<ProgramaDto>> ObtenerListaProgramaTodosEstado();
    }
}
