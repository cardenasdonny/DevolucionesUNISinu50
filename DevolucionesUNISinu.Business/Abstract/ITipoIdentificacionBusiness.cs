using DevolucionesUNISinu.Business.DTOs.Estudiantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface ITipoIdentificacionBusiness
    {
        Task<IEnumerable<TipoIdentificacionDto>> ObtenerListaTipoIdentificacionTodos();
        Task<IEnumerable<TipoIdentificacionDto>> ObtenerListaTipoIdentificacionTodosEstado();
        Task<TipoIdentificacionDto> ObtenerTipoIdentificacionPorId(int? id);
        void CrearTipoIdentificacion(TipoIdentificacionDto tipoIdentificacionDto);
        void EditarTipoIdentificacion(TipoIdentificacionDto tipoIdentificacionDto);
        Task CambiarEstado(int id);
        Task<bool> GuardarCambios();
    }
}
