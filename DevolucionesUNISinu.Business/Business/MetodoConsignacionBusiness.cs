using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Devoluciones;
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
    public class MetodoConsignacionBusiness: IMetodoConsignacionBusiness
    {
        private readonly AppDbContext _context;

        public MetodoConsignacionBusiness(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MetodoConsignacionDto>> ObtenerListaMetodoConsignacionTodos()
        {

            List<MetodoConsignacionDto> ListaMetodoConsignacionDto = new();
            var listaMetodoConsignacion = await _context.MetodosConsignacion.Where(x => x.Estado == true).OrderBy(x => x.Nombre).ToListAsync();
            listaMetodoConsignacion.ForEach(e =>
            {
                MetodoConsignacionDto metodoConsignacionDto = new()
                {
                    MetodoConsignacionId = e.MetodoConsignacionId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListaMetodoConsignacionDto.Add(metodoConsignacionDto);
            });
            return ListaMetodoConsignacionDto;
        }
        public async Task<IEnumerable<MetodoConsignacionDto>> ObtenerListaMetodoConsignacionTodosEstado()
        {

            List<MetodoConsignacionDto> ListaMetodoConsignacionDto = new();
            var listaMetodoConsignacion = await _context.MetodosConsignacion.OrderBy(x => x.Nombre).ToListAsync();
            listaMetodoConsignacion.ForEach(e =>
            {
                MetodoConsignacionDto metodoConsignacionDto = new()
                {
                    MetodoConsignacionId = e.MetodoConsignacionId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListaMetodoConsignacionDto.Add(metodoConsignacionDto);
            });
            return ListaMetodoConsignacionDto;
        }
        public async Task<MetodoConsignacionDto> ObtenerMetodoConsignacionPorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var metodoConsignacion = await _context.MetodosConsignacion.AsNoTracking().FirstOrDefaultAsync(x => x.MetodoConsignacionId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (metodoConsignacion != null)
            {
                MetodoConsignacionDto metodoConsignacionDto = new()
                {
                    MetodoConsignacionId = metodoConsignacion.MetodoConsignacionId,
                    Nombre = metodoConsignacion.Nombre,
                    Estado = metodoConsignacion.Estado,
                };
                return metodoConsignacionDto;
            }
            return null;
        }

        public void CrearMetodoConsignacion(MetodoConsignacionDto metodoConsignacionDto)
        {
            if (metodoConsignacionDto == null)
            {
                throw new ArgumentNullException(nameof(metodoConsignacionDto));
            }
            MetodoConsignacion metodoConsignacion = new()
            {
                Nombre = metodoConsignacionDto.Nombre,
                Estado = true,
            };
            _context.Add(metodoConsignacion);
        }
        public void EditarMetodoConsignacion(MetodoConsignacionDto metodoConsignacionDto)
        {
            if (metodoConsignacionDto == null)
            {
                throw new ArgumentNullException(nameof(metodoConsignacionDto));
            }

            MetodoConsignacion metodoConsignacion = new()
            {
                MetodoConsignacionId = metodoConsignacionDto.MetodoConsignacionId,
                Nombre = metodoConsignacionDto.Nombre,
                Estado = metodoConsignacionDto.Estado,
            };
            _context.Update(metodoConsignacion);

        }

        public async Task CambiarEstado(int id)
        {
            var metodoConsignacionDto = await ObtenerMetodoConsignacionPorId(id);
            if (metodoConsignacionDto.Estado == true)
                metodoConsignacionDto.Estado = false;
            else
                metodoConsignacionDto.Estado = true;

            MetodoConsignacion metodoConsignacion = new()
            {
                MetodoConsignacionId = metodoConsignacionDto.MetodoConsignacionId,
                Nombre = metodoConsignacionDto.Nombre,
                Estado = metodoConsignacionDto.Estado,
            };

            _context.Update(metodoConsignacion);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
