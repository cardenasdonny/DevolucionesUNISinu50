using DevolucionesUNISinu.Business.DTOs.Facultades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface ITipoProgramaBusiness
    {
        Task<IEnumerable<TipoProgramaDto>> ObtenerListaTipoProgramaTodos();
        Task<TipoProgramaDto> ObtenerTipoProgramaPorId(int? id);
        void CrearTipoPrograma(TipoProgramaDto tipoProgramaDto);
        void EditarTipoPrograma(TipoProgramaDto tipoProgramaDto);
        Task CambiarEstado(int id);
        Task<bool> GuardarCambios();

    }
}
