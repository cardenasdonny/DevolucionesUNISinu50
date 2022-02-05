using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Estudiantes;
using DevolucionesUNISinu.Model.DAL;
using DevolucionesUNISinu.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Business
{
    public class EstudianteBusiness: IEstudianteBusiness
    {
        private readonly AppDbContext _context;

        public EstudianteBusiness(AppDbContext context)
        {
            _context = context;
        }
        public void CrearEstudiante(EstudianteDto estudianteDto)
        {
            if (estudianteDto == null)
            {
                throw new ArgumentNullException(nameof(estudianteDto));
            }
            Estudiante estudiante  = new()
            {
                Nombres = estudianteDto.Nombres,
                Apellidos = estudianteDto.Apellidos,
                Identificacion = estudianteDto.Identificacion,
                TipoIdentificacionId = estudianteDto.TipoIdentificacionId.Value,
                Telefono = estudianteDto.Telefono,
                FacultadId = estudianteDto.FacultadId.Value,
                ProgramaId = estudianteDto.ProgramaId.Value,
                Semestre = estudianteDto.Semestre.Value,                
                Estado = true,
                UsuarioId = estudianteDto.UsuarioId
            };
            _context.Add(estudiante);
        }
        //Se le envia el UsuarioId y se obtiene el EstudianteId
        public async Task<int> ObtenerIdPorIdUsuario(string usuarioId)
        {
            //var a = await _context.Estudiantes.Where(y => y.UsuarioId.Equals(usuarioId)).Select(y => y.EstudianteId).DefaultIfEmpty().FirstAsync();
            return await _context.Estudiantes.Where(y => y.UsuarioId.Equals(usuarioId)).Select(y => y.EstudianteId).DefaultIfEmpty().FirstAsync();
        }

        public async Task<EstudianteDto> ObtenerEstudiantePorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var estudiante = await _context.Estudiantes.AsNoTracking().FirstOrDefaultAsync(x => x.EstudianteId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (estudiante != null)
            {
                EstudianteDto estudianteDto = new()
                {
                    EstudianteId = estudiante.EstudianteId,
                    Nombres = estudiante.Nombres,
                    Apellidos = estudiante.Apellidos,
                    Identificacion = estudiante.Identificacion,
                    TipoIdentificacionId = estudiante.TipoIdentificacionId,
                    Telefono = estudiante.Telefono,
                    FacultadId = estudiante.FacultadId,
                    ProgramaId = estudiante.ProgramaId,
                    Semestre = estudiante.Semestre,
                    Estado = true,
                    UsuarioId = estudiante.UsuarioId
                };
                return estudianteDto;
            }
            return null;
        }

        public async Task<EstudianteDetalleDto> ObtenerEstudianteDetallePorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var estudiante = await _context.Estudiantes.Include(x=>x.Usuario).Include(x=>x.TipoIdentificacion).Include(x=>x.Facultad).Include(x=>x.Programa).Include(x=>x.Programa.TipoPrograma).AsNoTracking().FirstOrDefaultAsync(x => x.EstudianteId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (estudiante != null)
            {
                EstudianteDetalleDto estudianteDetalleDto = new()
                {
                    Nombres = estudiante.Nombres,
                    Apellidos = estudiante.Apellidos,
                    Identificacion = estudiante.Identificacion,
                    TipoIdentificacion = estudiante.TipoIdentificacion.Nombre,
                    Telefono = estudiante.Telefono,
                    Facultad = estudiante.Facultad.Nombre,
                    Programa = estudiante.Programa.Nombre,
                    Semestre = estudiante.Semestre,
                    Estado = estudiante.Estado,
                    Email = estudiante.Usuario.Email,
                    TipoPrograma = estudiante.Programa.TipoPrograma.Nombre
                };
                return estudianteDetalleDto;
            }
            return null;
        }

        public void Editar(EstudianteDto estudianteDto)
        {
            if (estudianteDto == null)
            {
                throw new ArgumentNullException(nameof(estudianteDto));
            }

            Estudiante estudiante = new()
            {
                EstudianteId = estudianteDto.EstudianteId,
                Nombres = estudianteDto.Nombres,
                Apellidos = estudianteDto.Apellidos,
                Estado = estudianteDto.Estado,
                Telefono = estudianteDto.Telefono,
                TipoIdentificacionId = estudianteDto.TipoIdentificacionId.Value,
                Identificacion = estudianteDto.Identificacion,
                FacultadId = estudianteDto.FacultadId.Value,
                ProgramaId = estudianteDto.ProgramaId.Value,
                Semestre = estudianteDto.Semestre.Value,
                UsuarioId = estudianteDto.UsuarioId
            };
            _context.Update(estudiante);

        }


        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        /*
        public async Task<bool> Generar()
        {
            
                Estudiante estudiante = new()
                {
                    EstudianteId = 1,
                    Nombres = "Javier José",
                    Apellidos = "Padilla Cardona",
                    Estado= true,
                    ProgramaId = 1,
                    FacultadId= 4,
                    Telefono= "11223344",
                    TipoIdentificacionId = 1,
                    Identificacion = "112233445566",
                    UsuarioId = 
                    
                };
                _context.Add(estudiante);

            
            return await GuardarCambios();
        }
        */
    }
}
