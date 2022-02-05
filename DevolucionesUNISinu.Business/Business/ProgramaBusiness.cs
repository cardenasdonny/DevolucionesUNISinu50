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
    public class ProgramaBusiness:IProgramaBusiness
    {
        private readonly AppDbContext _context;

        public ProgramaBusiness(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProgramaDto>> ObtenerListaProgramaTodos()
        {

            List<ProgramaDto> ListaProgramaDto = new();
            var listaPrograma = await _context.Programas.Include(f=>f.Facultad).Include(t=>t.TipoPrograma).Where(x => x.Estado == true).OrderBy(x => x.Nombre).ToListAsync();
            listaPrograma.ForEach(e =>
            {
                ProgramaDto programaDto = new()
                {
                    ProgramaId = e.ProgramaId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                    NombreTipoPrograma = e.TipoPrograma.Nombre,
                    NombreFacultad = e.Facultad.Nombre,
                    Semestres = e.Semestres
                };
                ListaProgramaDto.Add(programaDto);
            });
            return ListaProgramaDto;
        }
        public async Task<IEnumerable<ProgramaDto>> ObtenerListaProgramaTodosEstado()
        {

            List<ProgramaDto> ListaProgramaDto = new();
            var listaPrograma = await _context.Programas.Include(f => f.Facultad).Include(t => t.TipoPrograma).OrderBy(x => x.Nombre).ToListAsync();
            listaPrograma.ForEach(e =>
            {
                ProgramaDto programaDto = new()
                {
                    ProgramaId = e.ProgramaId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                    NombreTipoPrograma = e.TipoPrograma.Nombre,
                    NombreFacultad = e.Facultad.Nombre,
                    Semestres = e.Semestres
                };
                ListaProgramaDto.Add(programaDto);
            });
            return ListaProgramaDto;
        }
        public async Task<ProgramaDto> ObtenerProgramaPorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var programa = await _context.Programas.Include(f => f.Facultad).Include(t => t.TipoPrograma).AsNoTracking().FirstOrDefaultAsync(x => x.ProgramaId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (programa != null)
            {
                ProgramaDto programaDto = new()
                {
                    ProgramaId = programa.ProgramaId,
                    Nombre = programa.Nombre,
                    Estado = programa.Estado,
                    NombreTipoPrograma = programa.TipoPrograma.Nombre,
                    NombreFacultad = programa.Facultad.Nombre,
                    TipoProgramaId = programa.TipoProgramaId,
                    FacultadId = programa.FacultadId,
                    Semestres = programa.Semestres
                };
                return programaDto;
            }
            return null;
        }

        public async Task<List<ProgramaDto>> ObtenerProgramasPorFacultad(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            List<ProgramaDto> ListaProgramaDto = new();
            var listaPrograma = await _context.Programas.Where(x=>x.FacultadId==id).Where(x => x.Estado == true).OrderBy(x => x.Nombre).ToListAsync();
            listaPrograma.ForEach(e =>
            {
                if (e != null) { 
                ProgramaDto programaDto = new()
                {
                    ProgramaId = e.ProgramaId,
                    Nombre = e.Nombre,                    
                };
                ListaProgramaDto.Add(programaDto);
                }
            });
            return ListaProgramaDto;
        }

        public async Task<List<SemestreDto>> ObtenerSemestresPorPrograma(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            List<SemestreDto> ListaSemestres = new();
            //var semestres = await _context.Programas.FindAsync(id).sele;
            var semestres = await _context.Programas.Where(x => x.ProgramaId == id).Select(x => x.Semestres).FirstOrDefaultAsync();
            for (int i = 0; i < semestres; i++)
            {
                SemestreDto semestreDto = new()
                {
                    Id = i + 1,
                    Semestre = i + 1
                };
                ListaSemestres.Add(semestreDto);
            }
            return ListaSemestres;
        }

        public void CrearPrograma(ProgramaDto programaDto)
        {
            if (programaDto == null)
            {
                throw new ArgumentNullException(nameof(programaDto));
            }
            Programa programa = new()
            {
                Nombre = programaDto.Nombre,
                FacultadId = programaDto.FacultadId.Value,
                ProgramaId = programaDto.ProgramaId,
                Semestres = programaDto.Semestres.Value,
                TipoProgramaId = programaDto.TipoProgramaId.Value,
                Estado = true,
            };
            _context.Add(programa);
        }
        public void EditarPrograma(ProgramaDto programaDto)
        {
            if (programaDto == null)
            {
                throw new ArgumentNullException(nameof(programaDto));
            }

            Programa programa = new()
            {
                ProgramaId = programaDto.ProgramaId,
                Nombre = programaDto.Nombre,
                Estado = programaDto.Estado,
                FacultadId = programaDto.FacultadId.Value,
                Semestres = programaDto.Semestres.Value,
                TipoProgramaId = programaDto.TipoProgramaId.Value
                
            };
            _context.Update(programa);

        }

        public async Task CambiarEstado(int id)
        {
            var programaDto = await ObtenerProgramaPorId(id);
            if (programaDto.Estado == true)
                programaDto.Estado = false;
            else
                programaDto.Estado = true;

            Programa programa = new()
            {
                Nombre = programaDto.Nombre,
                FacultadId = programaDto.FacultadId.Value,
                ProgramaId = programaDto.ProgramaId,
                Semestres = programaDto.Semestres.Value,
                TipoProgramaId = programaDto.TipoProgramaId.Value,
                Estado = programaDto.Estado,
            };
            _context.Update(programa);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
