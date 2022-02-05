using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business
{
    public class UsuarioClaimsPrincipalFactory : UserClaimsPrincipalFactory<Usuario, IdentityRole>
    {
        private readonly IEstudianteBusiness _estudianteBusiness;

        public UsuarioClaimsPrincipalFactory(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IEstudianteBusiness estudianteBusiness)
        : base(userManager, roleManager, optionsAccessor)
        {
            _estudianteBusiness = estudianteBusiness;
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
        {
            
            var identity = await base.GenerateClaimsAsync(user);
            var estudianteId = await _estudianteBusiness.ObtenerIdPorIdUsuario(user.Id);
            var estudiante = await _estudianteBusiness.ObtenerEstudiantePorId(estudianteId);
            string nombres;

            if (estudiante != null)
                nombres = estudiante.Nombres + " " + estudiante.Apellidos;
            else
                nombres = user.Email;


            identity.AddClaim(new Claim("Email", user.Email));
            identity.AddClaim(new Claim("EstudianteId", estudianteId.ToString()));
            identity.AddClaim(new Claim("Nombres", nombres));
            return identity;
        }
    }
}
