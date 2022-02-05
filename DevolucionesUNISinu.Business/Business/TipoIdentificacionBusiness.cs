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
    public class TipoIdentificacionBusiness: ITipoIdentificacionBusiness
    {
        private readonly AppDbContext _context;

        public TipoIdentificacionBusiness(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TipoIdentificacionDto>> ObtenerListaTipoIdentificacionTodos()
        {

            List<TipoIdentificacionDto> ListaTipoProgramaDto = new();
            var listatipoIdentificacion = await _context.TiposIdentificacion.Where(x=>x.Estado==true).OrderBy(x => x.Nombre).ToListAsync();
            listatipoIdentificacion.ForEach(e =>
            {
                TipoIdentificacionDto tipoIdentificacionDto = new()
                {
                    TipoIdentificacionId = e.TipoIdentificacionId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListaTipoProgramaDto.Add(tipoIdentificacionDto);
            });
            return ListaTipoProgramaDto;
        }
        public async Task<IEnumerable<TipoIdentificacionDto>> ObtenerListaTipoIdentificacionTodosEstado()
        {

            List<TipoIdentificacionDto> ListaTipoProgramaDto = new();
            var listatipoIdentificacion = await _context.TiposIdentificacion.OrderBy(x => x.Nombre).ToListAsync();
            listatipoIdentificacion.ForEach(e =>
            {
                TipoIdentificacionDto tipoIdentificacionDto = new()
                {
                    TipoIdentificacionId = e.TipoIdentificacionId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListaTipoProgramaDto.Add(tipoIdentificacionDto);
            });
            return ListaTipoProgramaDto;
        }
        public async Task<TipoIdentificacionDto> ObtenerTipoIdentificacionPorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var tipoIdentificacion = await _context.TiposIdentificacion.AsNoTracking().FirstOrDefaultAsync(x => x.TipoIdentificacionId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (tipoIdentificacion != null)
            {
                TipoIdentificacionDto tipoIdentificacionDto = new()
                {
                    TipoIdentificacionId = tipoIdentificacion.TipoIdentificacionId,
                    Nombre = tipoIdentificacion.Nombre,
                    Estado = tipoIdentificacion.Estado,
                };
                return tipoIdentificacionDto;
            }
            return null;
        }

        public void CrearTipoIdentificacion(TipoIdentificacionDto tipoIdentificacionDto)
        {
            if (tipoIdentificacionDto == null)
            {
                throw new ArgumentNullException(nameof(tipoIdentificacionDto));
            }
            TipoIdentificacion tipoIdentificacion = new()
            {
                Nombre = tipoIdentificacionDto.Nombre,
                Estado = true,
            };
            _context.Add(tipoIdentificacion);
        }
        public void EditarTipoIdentificacion(TipoIdentificacionDto tipoIdentificacionDto)
        {
            if (tipoIdentificacionDto == null)
            {
                throw new ArgumentNullException(nameof(tipoIdentificacionDto));
            }

            TipoIdentificacion tipoIdentificacion = new()
            {
                TipoIdentificacionId = tipoIdentificacionDto.TipoIdentificacionId,
                Nombre = tipoIdentificacionDto.Nombre,
                Estado = tipoIdentificacionDto.Estado,
            };
            _context.Update(tipoIdentificacion);

        }

        public async Task CambiarEstado(int id)
        {
            var tipoIdentificacionDto = await ObtenerTipoIdentificacionPorId(id);
            if (tipoIdentificacionDto.Estado == true)
                tipoIdentificacionDto.Estado = false;
            else
                tipoIdentificacionDto.Estado = true;

            TipoIdentificacion tipoIdentificacion = new()
            {
                TipoIdentificacionId = tipoIdentificacionDto.TipoIdentificacionId,
                Nombre = tipoIdentificacionDto.Nombre,
                Estado = tipoIdentificacionDto.Estado,
            };

            _context.Update(tipoIdentificacion);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
