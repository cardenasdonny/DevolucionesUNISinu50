using DevolucionesUNISinu.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.DAL
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
           
            //Se crean los roles por defecto

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "02184cf0–9412–4cfe-afbf-51f706h72cf6",
                    Name = "Administrador",
                    NormalizedName = "ADMINISTRADOR"
                },
                new IdentityRole
                {
                    Id = "625h8gf0–9y12–4cfe-afbf-41f70yh72bf3",
                    Name = "Observador",
                    NormalizedName = "OBSERVADOR"
                },
                new IdentityRole
                {
                    Id = "621h8cf0–9y12–4cfe-afbf-51f70uh72cf5",
                    Name = "Estudiante",
                    NormalizedName = "ESTUDIANTE"
                },
                new IdentityRole
                {
                    Id = "621u8cf0–8y12–8cde-afbf-f1k70uh72hf5",
                    Name = "ApoyoFinanciero",
                    NormalizedName = "APOYOFINANCIERO"
                },
                new IdentityRole
                {
                    Id = "621p8cf0–8y12–4cde-afbf-f1t70uh72hf5",
                    Name = "Contabilidad",
                    NormalizedName = "CONTABILIDAD"
                },
                new IdentityRole
                {
                    Id = "2w1p8cf0–7gy12–4cde-sfbf-f1k71uh72hf5",
                    Name = "Tesoreria",
                    NormalizedName = "TESORERIA"
                }
            );

            //crear usuario ADMINISTRADOR
            var appUser = new Usuario
            {
                Id = "0h174cfb–4418–1c3e-a2bf-89f716w72cu3",
                Email = "dony.cardenas@xofsystems.com",
                NormalizedEmail = "DONY.CARDENAS@XOFSYSTEMS.COM",                
                UserName = "dony.cardenas@xofsystems.com",
                NormalizedUserName = "DONY.CARDENAS@XOFSYSTEMS.COM",
                EmailConfirmed = true,
                Estado = true,
                Nombres = "Admin",
                Apellidos = "Admin"              
                
            };
            //encriptacion password
            PasswordHasher<Usuario> ph = new PasswordHasher<Usuario>();
            appUser.PasswordHash = ph.HashPassword(appUser, "1234567890");
            //se guarda el user
            modelBuilder.Entity<Usuario>().HasData(appUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "02184cf0–9412–4cfe-afbf-51f706h72cf6", UserId = "0h174cfb–4418–1c3e-a2bf-89f716w72cu3" }
                );

            /*


            //crear usuario Estudiante
            appUser = new Usuario
            {
                Id = "8o184cf1–5218–1d3e-b5bf-8vf716w78cy1",
                Email = "cardenasdonny@hotmail.com",
                NormalizedEmail = "CARDENASDONNY@HOTMAIL.COM",
                UserName = "cardenasdonny@hotmail.com",
                NormalizedUserName = "CARDENASDONNY@HOTMAIL.COM",
                EmailConfirmed = true,
                Estado = true,
                Nombres = "Estudiante",
                Apellidos = "Estudiante"
            };
            appUser.PasswordHash = ph.HashPassword(appUser, "1234567890");
            //se guarda el user
            modelBuilder.Entity<Usuario>().HasData(appUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "621h8cf0–9y12–4cfe-afbf-51f70uh72cf5", UserId = "8o184cf1–5218–1d3e-b5bf-8vf716w78cy1" }
                );

            //Estudiantes prueba

            modelBuilder.Entity<Estudiante>().HasData(
                new Estudiante
                {
                    EstudianteId = 1,
                    Nombres = "Javier José",
                    Apellidos = "Padilla Cardona",
                    Estado = true,
                    ProgramaId = 1,
                    FacultadId = 4,
                    Telefono = "11223344",
                    TipoIdentificacionId = 1,
                    Identificacion = "112233445566",
                    UsuarioId = "8o184cf1–5218–1d3e-b5bf-8vf716w78cy1",
                    Semestre = 4
                }
            );



            //crear usuario APOYOFINANCIERO
            appUser = new Usuario
           {
               Id = "2o184cf1–5218–1d3e-b5bf-8vf716w78cy1",
               Email = "ddcardenas02@misena.edu.co",
               NormalizedEmail = "DDCARDENAS02@MISENA.EDU.CO",
               UserName = "ddcardenas02@misena.edu.co",
               NormalizedUserName = "DDCARDENAS02@MISENA.EDU.CO",
               EmailConfirmed = true,               
               Estado = true,
               Nombres = "Apoyo Financiero",
               Apellidos = "Apoyo Financiero"
           };
            appUser.PasswordHash = ph.HashPassword(appUser, "1234567890");
           //se guarda el user
           modelBuilder.Entity<Usuario>().HasData(appUser);
           modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>() { RoleId = "621u8cf0–8y12–8cde-afbf-f1k70uh72hf5", UserId = "2o184cf1–5218–1d3e-b5bf-8vf716w78cy1" }
               );

            


           //crear usuario CONTABILIDAD
           appUser = new Usuario
           {
               Id = "41851gx1–f258–1f4t-b7bf-pbf786w78ck1",
               Email = "pruebas@xofsystems.com",
               NormalizedEmail = "PRUEBAS@XOFSYSTEMS.COM",
               UserName = "pruebas@xofsystems.com",
               NormalizedUserName = "PRUEBAS@XOFSYSTEMS.COM",
               EmailConfirmed = true,
               Estado = true,
               PasswordHash = ph.HashPassword(appUser, "1234567890"),
               Nombres = "Contabilidad",
               Apellidos = "Contabilidad"
           };                      
           //se guarda el user
           modelBuilder.Entity<Usuario>().HasData(appUser);
           modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>() { RoleId = "621p8cf0–8y12–4cde-afbf-f1t70uh72hf5", UserId = "41851gx1–f258–1f4t-b7bf-pbf786w78ck1" }
               );

            //crear usuario TESORERIA
            appUser = new Usuario
            {
                Id = "15851tx1–f258–1f5t-t7bf-pbf786t78ck1",
                Email = "xofsystems@xofsystems.com",
                NormalizedEmail = "XOFSYSTEMS@XOFSYSTEMS.COM",
                UserName = "xofsystems@xofsystems.com",
                NormalizedUserName = "XOFSYSTEMS@XOFSYSTEMS.COM",
                EmailConfirmed = true,
                Estado = true,
                PasswordHash = ph.HashPassword(appUser, "1234567890"),
                Nombres = "Tesoreria",
                Apellidos = "Tesoreria"
            };
            //se guarda el user
            modelBuilder.Entity<Usuario>().HasData(appUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "2w1p8cf0–7gy12–4cde-sfbf-f1k71uh72hf5", UserId = "15851tx1–f258–1f5t-t7bf-pbf786t78ck1" }
                );

            */

            modelBuilder.Entity<Facultad>().HasData(
                new Facultad
                {
                    FacultadId = 1,
                    Nombre = "Facultad de Ciencias e Ingenierías",
                    Estado = true
                },
                new Facultad
                {
                    FacultadId = 2,
                    Nombre = "Facultad de Ciencias Jurídicas, Sociales y Educación",
                    Estado = true
                },
                new Facultad
                {
                    FacultadId = 3,
                    Nombre = "Facultad de Ciencias de la Salud",
                    Estado = true
                },
                new Facultad
                {
                    FacultadId = 4,
                    Nombre = "Facultad de Ciencias Económicas, Administrativas y Contables",
                    Estado = true
                },
                new Facultad
                {
                    FacultadId = 5,
                    Nombre = "Facultad de Ciencias Humanas, Arte y Diseño",
                    Estado = true
                }                
            );

            modelBuilder.Entity<Banco>().HasData(
                new Banco
                {
                    BancoId = 1,
                    Nombre = "Banco Agrario de Colombia",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 2,
                    Nombre = "Banco AV Villas",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 3,
                    Nombre = "Banco BBVA",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 4,
                    Nombre = "Banco BCSC",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 5,
                    Nombre = "Banco Citibank",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 6,
                    Nombre = "Banco Coopcentral",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 7,
                    Nombre = "Banco Davivienda",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 8,
                    Nombre = "Banco de Bogotá",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 9,
                    Nombre = "Banco de la República",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 10,
                    Nombre = "Banco de Occidente",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 11,
                    Nombre = "Banco Falabella",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 12,
                    Nombre = "Banco Finandina",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 13,
                    Nombre = "Banco GNB Sudameris",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 14,
                    Nombre = "Banco Itaú Corpbanca Colombia S.A.",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 16,
                    Nombre = "Banco Mundo Mujer",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 17,
                    Nombre = "Banco Pichincha",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 18,
                    Nombre = "Banco Popular",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 19,
                    Nombre = "Banco Procredit Colombia",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 15,
                    Nombre = "Banco Santander de Negocios Colombia S.A.",
                    Estado = true
                },
                new Banco
                {
                    BancoId = 20,
                    Nombre = "BancoColombia",
                    Estado = true
                }
            );

            modelBuilder.Entity<ConceptoDevolucion>().HasData(
                new ConceptoDevolucion
                {
                    ConceptoDevolucionId = 1,
                    Nombre = "Retiro voluntario",
                    DocumentosRequeridos = "Para el trámite de devolución por retiro voluntario (art 51 reglamento estudiantil) el estudiante debe adjuntar:\n\n- Carta de retiro presentada en su Dirección de Escuela.\n- Carta de aceptación de Retiro voluntario R-AMRC-031.\n- Formato de retiro voluntario R-AMRC-023.\n- Formulario para tramitar devolución R-SEC-004 (Se debe especificar que la cuenta debe estar a nombre del estudiante, no aplica daviplata, nequi o ahorro a la mano, dado que no se realiza devolución a terceros).\n- Certificado cuenta bancaria.",
                    Estado = true
                },
                new ConceptoDevolucion
                {
                    ConceptoDevolucionId = 2,
                    DocumentosRequeridos = "El estudiante que paga su matrícula en efectivo y posterior una entidad realiza nuevamente el pago (Icetex- Alcaldía de Cartagena- Empresas donde laboran los padres de familia), una vez la entidad les notifique el giro el estudiante puede solicitar su trámite de devolución adjuntando:\n\n- Copia del soporte de pago realizado por el estudiante.\n- Soporte del pago de la entidad.\n- Copia del documento de identidad.\n- Formulario para tramitar devolución R-SEC-004 (Se debe especificar que la cuenta debe estar a nombre del estudiante, no aplica Daviplata, Nequi o ahorro a la mano, dado que no se realiza devolución a terceros).\n- Certificado cuenta bancaria.",
                    Nombre = "Pago doble de matrícula",
                    Estado = true
                },
                new ConceptoDevolucion
                {
                    ConceptoDevolucionId = 3,
                    DocumentosRequeridos = "El estudiante que realiza un pago mayor del facturado o los estudiantes que tienen financiación Unicred y posterior a esto les gira Icetex se le devuelve el saldo de los abonos que hayan realizado hasta la fecha del giro, debe anexar:\n\n- Copia del soporte de pago realizado por el estudiante.\n- Copia del documento de identidad.\n- Formulario para tramitar devolución R-SEC-004 (Se debe especificar que la cuenta debe estar a nombre del estudiante, no aplica Daviplata, Nequi o ahorro a la mano, dado que no se realiza devolución a terceros).\n- Certificado cuenta bancaria.",
                    Nombre = "Saldo a favor",
                    Estado = true
                },
                new ConceptoDevolucion
                {
                    ConceptoDevolucionId = 4,
                    DocumentosRequeridos = "El estudiante que realiza un pago mayor del facturado o los estudiantes que tienen financiación Unicred y posterior a esto les gira Icetex se le devuelve el saldo de los abonos que hayan realizado hasta la fecha del giro, debe anexar:\n\n- Copia del soporte de pago realizado por el estudiante.\n- Copia del documento de identidad.\n- Formulario para tramitar devolución R-SEC-004 (Se debe especificar que la cuenta debe estar a nombre del estudiante, no aplica Daviplata, Nequi o ahorro a la mano, dado que no se realiza devolución a terceros).\n- Certificado cuenta bancaria.",
                    Nombre = "Devolución por no apertura de cursos",
                    Estado = true
                },
                new ConceptoDevolucion
                {
                    ConceptoDevolucionId = 5,
                    DocumentosRequeridos = "El estudiante que realiza un pago mayor del facturado o los estudiantes que tienen financiación Unicred y posterior a esto les gira Icetex se le devuelve el saldo de los abonos que hayan realizado hasta la fecha del giro, debe anexar:\n\n- Copia del soporte de pago realizado por el estudiante.\n- Copia del documento de identidad.\n- Formulario para tramitar devolución R-SEC-004 (Se debe especificar que la cuenta debe estar a nombre del estudiante, no aplica Daviplata, Nequi o ahorro a la mano, dado que no se realiza devolución a terceros).\n- Certificado cuenta bancaria.",
                    Nombre = "Devolución pago de Inscripción",
                    Estado = true
                },
                new ConceptoDevolucion
                {
                    ConceptoDevolucionId = 6,
                    DocumentosRequeridos = "Adjunte los documentos necesarios para su tramitre de solicitud.",
                    Nombre = "Otro",
                    Estado = true
                }
            ); 
            modelBuilder.Entity<MetodoConsignacion>().HasData(
                new MetodoConsignacion
                {
                    MetodoConsignacionId = 1,
                    Nombre = "Cuenta de ahorros",
                    
                    Estado = true
                },
                new MetodoConsignacion
                {
                    MetodoConsignacionId = 2,
                    Nombre = "Cuenta corriente",
                    Estado = true
                }
            );
            modelBuilder.Entity<TipoIdentificacion>().HasData(
                new TipoIdentificacion
                {
                    TipoIdentificacionId = 1,
                    Nombre = "Tarjeta de identidad",
                    Estado = true
                },
                new TipoIdentificacion
                {
                    TipoIdentificacionId = 2,
                    Nombre = "Cédula de ciudadanía",
                    Estado = true
                },
                new TipoIdentificacion
                {
                    TipoIdentificacionId = 3,
                    Nombre = "Cédula de extranjería",
                    Estado = true
                }
            );
            modelBuilder.Entity<TipoPrograma>().HasData(
                new TipoPrograma
                {
                    TipoProgramaId = 1,
                    Nombre = "Pregrado",
                    Estado = true
                },
                new TipoPrograma
                {
                    TipoProgramaId = 2,
                    Nombre = "Postgrado",
                    Estado = true
                },
                new TipoPrograma
                {
                    TipoProgramaId = 3,
                    Nombre = "Diplomado",
                    Estado = true
                },
                new TipoPrograma
                {
                    TipoProgramaId = 4,
                    Nombre = "Educación continuada",
                    Estado = true
                }
            );
            modelBuilder.Entity<Programa>().HasData(
                new Programa
                {
                    ProgramaId = 1,
                    TipoProgramaId = 1,
                    Nombre = "Administración de Empresas",
                    Estado = true,
                    FacultadId = 4,
                    Semestres = 8,                    
                },
                new Programa
                {
                    ProgramaId = 2,
                    TipoProgramaId = 1,
                    Nombre = "Arquitectura",
                    Estado = true,
                    FacultadId = 5,
                    Semestres = 10,
                },
                new Programa
                {
                    ProgramaId = 3,
                    TipoProgramaId = 1,
                    Nombre = "Comunicación Social",
                    Estado = true,
                    FacultadId = 5,
                    Semestres = 8,
                },
                new Programa
                {
                    ProgramaId = 4,
                    TipoProgramaId = 1,
                    Nombre = "Contaduría Pública",
                    Estado = true,
                    FacultadId = 4,
                    Semestres = 9,
                },
                new Programa
                {
                    ProgramaId = 5,
                    TipoProgramaId = 1,
                    Nombre = "Derecho",
                    Estado = true,
                    FacultadId = 2,
                    Semestres = 10,
                }

            );
            

            
            
            
        }
    }
}
