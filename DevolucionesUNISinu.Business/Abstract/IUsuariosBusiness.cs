using DevolucionesUNISinu.Business.DTOs.Usuarios;
using DevolucionesUNISinu.Model.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IUsuariosBusiness
    {
        Task<IEnumerable<UsuarioDto>> ObtenerListaUsuarioTodos();
        Task<string> CrearUsuario(UsuarioRegistrarDto UsuarioRegistrarDto);
        Task<UsuarioDto> ObtenerUsuarioDtoPorEmail(string email);
        Task<Usuario> ObtenerUsuarioPorId(string id);
        Task CambiarEstado(string id);
        Task<string> ActualizarUsuario(UsuarioEditarDto usuarioEditarDto);
        Task<UsuarioEditarDto> ObtenerUsuarioDtoPorId(string id);
        Task<string> ActualizarEmail(string id, string email);
    }
}
