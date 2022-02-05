using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.DTOs.Roles;
using DevolucionesUNISinu.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Business
{
    public class RolBusiness: IRolBusiness
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Usuario> _userManager;

        public RolBusiness(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AsignarRolPorId(string id, string rol)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var usuario = await _userManager.FindByIdAsync(id);
                      
            
            if (usuario != null) {
                var roles = await _userManager.GetRolesAsync(usuario);

                await _userManager.RemoveFromRolesAsync(usuario, roles.ToArray());
                var result = await _userManager.AddToRoleAsync(usuario, rol);
                if (result.Succeeded)
                    return true;
                else
                    return false;
            }else
                return false;
        }

        public async Task<string> ObtenerRolUsuario(Usuario usuario)
        {
            var roles = await _userManager.GetRolesAsync(usuario);            
            return roles.FirstOrDefault();
        }

        public async Task<List<RolDto>> ObtenerListaRolesTodos()
        {
            List<RolDto> ListaRolDtos = new();
            var listaIdentityRole = await _roleManager.Roles.ToListAsync();
            if (listaIdentityRole != null)
            {
                listaIdentityRole.ForEach(e =>
                {
                    RolDto rolDto = new()
                    {
                        Id = e.Id,
                        Nombre = e.Name
                    };
                    ListaRolDtos.Add(rolDto);
                });
                return ListaRolDtos;
            }            

            return null;
       
        }



        public async Task<List<RolDto>> ObtenerListaRolesAdministradoresObservadores()
        {
            List<RolDto> ListaRolDtos = new();
            var listaIdentityRole = await _roleManager.Roles.Where(x=>!x.Name.Equals("Estudiante")).ToListAsync();
            if (listaIdentityRole != null)
            {
                listaIdentityRole.ForEach(e =>
                {
                    RolDto rolDto = new()
                    {
                        Id = e.Id,
                        Nombre = e.Name
                    };
                    ListaRolDtos.Add(rolDto);
                });
                return ListaRolDtos;
            }

            return null;

        }
    }
}
