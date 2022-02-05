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
    public class TipoProgramaBusinesss: ITipoProgramaBusiness
    {
        private readonly AppDbContext _context;

        public TipoProgramaBusinesss(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TipoProgramaDto>> ObtenerListaTipoProgramaTodos()
        {

            List<TipoProgramaDto> ListaTipoProgramaDto = new();
            var listaTipoPrograma = await _context.TiposPrograma.OrderBy(x => x.Nombre).ToListAsync();
            listaTipoPrograma.ForEach(e =>
            {
                TipoProgramaDto tipoProgramaDto = new()
                {
                    TipoProgramaId = e.TipoProgramaId,
                    Nombre = e.Nombre,
                    Estado = e.Estado,
                };
                ListaTipoProgramaDto.Add(tipoProgramaDto);
            });
            return ListaTipoProgramaDto;
        }
        public async Task<TipoProgramaDto> ObtenerTipoProgramaPorId(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var tipoPrograma = await _context.TiposPrograma.AsNoTracking().FirstOrDefaultAsync(x => x.TipoProgramaId == id);
            //var facultad = await _context.Facultades.FirstOrDefaultAsync(x => x.FacultadId == id);
            if (tipoPrograma != null)
            {
                TipoProgramaDto tipoProgramaDto = new()
                {
                    TipoProgramaId = tipoPrograma.TipoProgramaId,
                    Nombre = tipoPrograma.Nombre,
                    Estado = tipoPrograma.Estado,
                };
                return tipoProgramaDto;
            }
            return null;
        }

        public void CrearTipoPrograma(TipoProgramaDto tipoProgramaDto)
        {
            if (tipoProgramaDto == null)
            {
                throw new ArgumentNullException(nameof(tipoProgramaDto));
            }
            TipoPrograma tipoPrograma = new()
            {
                Nombre = tipoProgramaDto.Nombre,
                Estado = true,
            };
            _context.Add(tipoPrograma);
        }
        public void EditarTipoPrograma(TipoProgramaDto tipoProgramaDto)
        {
            if (tipoProgramaDto == null)
            {
                throw new ArgumentNullException(nameof(tipoProgramaDto));
            }

            TipoPrograma tipoPrograma = new()
            {
                TipoProgramaId = tipoProgramaDto.TipoProgramaId,
                Nombre = tipoProgramaDto.Nombre,
                Estado = tipoProgramaDto.Estado,
            };
            _context.Update(tipoPrograma);

        }

        public async Task CambiarEstado(int id)
        {
            var tipoProgramaDto = await ObtenerTipoProgramaPorId(id);
            if (tipoProgramaDto.Estado == true)
                tipoProgramaDto.Estado = false;
            else
                tipoProgramaDto.Estado = true;

            TipoPrograma tipoPrograma = new()
            {
                TipoProgramaId = tipoProgramaDto.TipoProgramaId,
                Nombre = tipoProgramaDto.Nombre,
                Estado = tipoProgramaDto.Estado,
            };

            _context.Update(tipoPrograma);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

