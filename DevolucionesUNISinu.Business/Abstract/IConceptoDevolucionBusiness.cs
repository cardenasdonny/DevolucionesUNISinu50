using DevolucionesUNISinu.Business.DTOs.Devoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IConceptoDevolucionBusiness
    {
        Task<IEnumerable<ConceptoDevolucionDto>> ObtenerListaConceptoDevolucionTodos();
        Task<ConceptoDevolucionDto> ObtenerConceptoDevolucionPorId(int? id);
        void CrearConceptoDevolucion(ConceptoDevolucionDto conceptoDevolucionDto);
        void EditarConceptoDevolucion(ConceptoDevolucionDto conceptoDevolucionDto);
        Task CambiarEstado(int id);
        Task<bool> GuardarCambios();
        Task<IEnumerable<ConceptoDevolucionDto>> ObtenerListaConceptoDevolucionTodosEstado();



    }
}
