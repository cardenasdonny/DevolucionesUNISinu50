using DevolucionesUNISinu.Business.DTOs.Estudiantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IEstudianteBusiness
    {
        void CrearEstudiante(EstudianteDto estudianteDto);
        Task<int> ObtenerIdPorIdUsuario(string usuarioId);
        Task<EstudianteDto> ObtenerEstudiantePorId(int? id);
        Task<EstudianteDetalleDto> ObtenerEstudianteDetallePorId(int? id);
        Task<bool> GuardarCambios();
        void Editar(EstudianteDto estudianteDto);
    }
}
