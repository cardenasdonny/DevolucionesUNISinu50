using DevolucionesUNISinu.Business.DTOs.Facultades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IFactultadBusiness
    {
        Task<IEnumerable<FacultadDto>> ObtenerListaFacultadTodas();
        Task<FacultadDto> ObtenerFacultadPorId(int? id);
        void CrearFacultad(FacultadDto FacultadDto);
        void EditarFacultad(FacultadDto FacultadDto);
        Task<bool> GuardarCambios();
        Task CambiarEstado(int id);
        Task<IEnumerable<FacultadDto>> ObtenerListaFacultadTodasEstado();

    }
}
