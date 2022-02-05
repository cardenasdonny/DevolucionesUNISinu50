using DevolucionesUNISinu.Business.DTOs.Devoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IBancoBusiness
    {
        Task<IEnumerable<BancoDto>> ObtenerListaBancoTodos();
        Task<BancoDto> ObtenerBancoPorId(int? id);
        void CrearBanco(BancoDto bancoDto);
        void EditarBanco(BancoDto bancoDto);
        Task CambiarEstado(int id);
        Task<bool> GuardarCambios();
        Task<IEnumerable<BancoDto>> ObtenerListaBancoTodosEstados();
    }
}
