using DevolucionesUNISinu.Business.DTOs.Roles;
using DevolucionesUNISinu.Model.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Abstract
{
    public interface IRolBusiness
    {
        Task<string> ObtenerRolUsuario(Usuario usuario);
        Task<bool> AsignarRolPorId(string id, string rol);
        Task<List<RolDto>> ObtenerListaRolesTodos();
        Task<List<RolDto>> ObtenerListaRolesAdministradoresObservadores();
    }
}
