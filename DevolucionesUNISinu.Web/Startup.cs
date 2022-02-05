using DevolucionesUNISinu.Business;
using DevolucionesUNISinu.Business.Abstract;
using DevolucionesUNISinu.Business.Business;
using DevolucionesUNISinu.Model.DAL;
using DevolucionesUNISinu.Model.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();

            //Db            
            var conexion = Configuration["ConnectionStrings:conexion_MySQL"];
            services.AddDbContext<AppDbContext>(options => options.UseMySql(conexion, ServerVersion.AutoDetect(conexion)));
            /*
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 12));
            
            services.AddDbContext<AppDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(conexion, serverVersion)
                .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                .EnableDetailedErrors()       // <-- with debugging (remove for production).
        );
            */


            services.AddScoped<IFactultadBusiness, FactultadBusiness>();
            services.AddScoped<IBancoBusiness, BancoBusiness>();
            services.AddScoped<IConceptoDevolucionBusiness, ConceptoDevolucionBusiness>();
            services.AddScoped<IMetodoConsignacionBusiness, MetodoConsignacionBusiness>();
            services.AddScoped<ITipoIdentificacionBusiness, TipoIdentificacionBusiness>();       
            services.AddScoped<IProgramaBusiness, ProgramaBusiness>();
            services.AddScoped<ITipoProgramaBusiness, TipoProgramaBusinesss>();
            services.AddScoped<IEstudianteBusiness, EstudianteBusiness>();
            services.AddScoped<IUsuariosBusiness, UsuarioBusiness>();
            services.AddScoped<IRolBusiness, RolBusiness>();
            services.AddScoped<IEmailBusiness, EmailBusiness>();            
            services.AddScoped<IDevolucionBusiness, DevolucionBusiness>();
            services.AddScoped<IDashBoardBusiness, DashBoardBusiness>();
            //Indentity

            services.AddIdentity<Usuario, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
              //.AddDefaultUI()
              .AddDefaultTokenProviders() //para trabajar con la confirmación de email
              .AddEntityFrameworkStores<AppDbContext>()       
              .AddClaimsPrincipalFactory<UsuarioClaimsPrincipalFactory>();

            //configuración del password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                //options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 9;
                options.User.RequireUniqueEmail = true;
            });

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(5);//You can set Time   
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Admin/NoAutorizado");
                options.Cookie.Name = "Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.LoginPath = new PathString("/Usuarios/Login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Admin/Error");               
                app.UseStatusCodePagesWithReExecute("/Admin/HttpStatusCodeHandler", "?code={0}");
              
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Facultades}/{action=Index}/{id?}");
            });
            */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Usuarios}/{action=Login}/{id?}");
            });



        }
    }
}
