using DevolucionesUNISinu.Business.DTOs.Devoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IMetodoConsignacionBusiness
    {
        Task<IEnumerable<MetodoConsignacionDto>> ObtenerListaMetodoConsignacionTodos();
        Task<MetodoConsignacionDto> ObtenerMetodoConsignacionPorId(int? id);
        void CrearMetodoConsignacion(MetodoConsignacionDto metodoConsignacionDto);
        void EditarMetodoConsignacion(MetodoConsignacionDto metodoConsignacionDto);
        Task CambiarEstado(int id);
        Task<bool> GuardarCambios();
        Task<IEnumerable<MetodoConsignacionDto>> ObtenerListaMetodoConsignacionTodosEstado();
    }
}
