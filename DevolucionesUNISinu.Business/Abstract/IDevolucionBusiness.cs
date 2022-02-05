using DevolucionesUNISinu.Business.DTOs.Devoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IDevolucionBusiness
    {
        Task<int?> Crear(DevolucionCrearDto devolucionDto, int estudianteId);
        void CrearDevolucionDetalleArchivo(int devolucionId, string nombreArchivo);
        Task<IEnumerable<DevolucionEncabezadoDto>> ObtenerListaEncabezadoDevoluciones(int estado, int estudianteId, string rol);
        Task<DevolucionDto> ObtenerDevolucionPorId(int? id, string rol);
        public string ObtenerEstado(int estado, string rol);
        Task<bool> Generar();
        Task<bool> GuardarCambios();
        Task<bool> Responder(DevolucionDto devolucionDto, string usuarioId);
        //void CrearDevolucionDetalleRespuesta(int devolucionId, string nombreArchivo);
    }
}
