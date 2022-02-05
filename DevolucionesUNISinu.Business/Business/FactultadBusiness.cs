using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Facultades;
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
    public class FactultadBusiness: IFactultadBusiness
    {
        private readonly AppDbContext _context;      

        public FactultadBusiness(AppDbContext context)
        {
            _context = context;            
        }

        public async Task<IEnumerable<FacultadDto>> ObtenerListaFacultadTodas()
        {
            
            List<FacultadDto> ListafacultadDtos = new();
            var listaFacultades = await _context.Facultades.Where(x => x.Estado == true).OrderBy(x => x.Nombre).ToListAsync();
            listaFacultades.ForEach(e =>
            {
                FacultadDto facultadDto = new()
                {
                    FacultadId = e.FacultadId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListafacultadDtos.Add(facultadDto);
            });
            return ListafacultadDtos;
        }
        public async Task<IEnumerable<FacultadDto>> ObtenerListaFacultadTodasEstado()
        {

            List<FacultadDto> ListafacultadDtos = new();
            var listaFacultades = await _context.Facultades.OrderBy(x => x.Nombre).ToListAsync();
            listaFacultades.ForEach(e =>
            {
                FacultadDto facultadDto = new()
                {
                    FacultadId = e.FacultadId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListafacultadDtos.Add(facultadDto);
            });
            return ListafacultadDtos;
        }
        public async Task<FacultadDto> ObtenerFacultadPorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var facultad = await _context.Facultades.AsNoTracking().FirstOrDefaultAsync(x => x.FacultadId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (facultad != null)
            {
                FacultadDto facultadDto = new()
                {
                    FacultadId = facultad.FacultadId,
                    Nombre = facultad.Nombre,
                    Estado = facultad.Estado,
                };
                return facultadDto;
            }
            return null;
        }

        public void CrearFacultad(FacultadDto facultadDto)
        {
            if (facultadDto == null)
            {
                throw new ArgumentNullException(nameof(facultadDto));
            }
            Facultad facultad = new()
            {
                Nombre = facultadDto.Nombre,
                Estado = true,
            };
            _context.Add(facultad);            
        }
        public void EditarFacultad(FacultadDto facultadDto)
        {
            if (facultadDto == null)
            {
                throw new ArgumentNullException(nameof(FacultadDto));
            }

            Facultad facultad = new()
            {
                FacultadId = facultadDto.FacultadId,
                Nombre = facultadDto.Nombre,
                Estado = facultadDto.Estado,                
            };
            _context.Update(facultad);    
            
        }

        public async Task CambiarEstado(int id)
        {
            var facultadDto = await ObtenerFacultadPorId(id);
            if (facultadDto.Estado == true)
                facultadDto.Estado = false;
            else
                facultadDto.Estado = true;

            Facultad facultad = new()
            {
                FacultadId = facultadDto.FacultadId,
                Nombre = facultadDto.Nombre,
                Estado = facultadDto.Estado,
            };
           
             _context.Update(facultad);  
        }

        public async Task<bool> GuardarCambios()
        {            
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
