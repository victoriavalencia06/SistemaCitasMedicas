using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaCitasMedicas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SistemaCitasMedicas.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        // DbSets de todas las entidades
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<DoctorEspecializacion> DoctorEspecializaciones { get; set; }
        public DbSet<Especializacion> Especializaciones { get; set; }
        public DbSet<HistorialMedico> HistorialesMedicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Nuevos DbSets
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RolMenu> RolMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Asignar nombre de tablas
            modelBuilder.Entity<Cita>().ToTable("t_cita");
            modelBuilder.Entity<Doctor>().ToTable("t_doctor");
            modelBuilder.Entity<DoctorEspecializacion>().ToTable("t_doctor_especializacion");
            modelBuilder.Entity<Especializacion>().ToTable("t_especializacion");
            modelBuilder.Entity<HistorialMedico>().ToTable("t_historial_medico");
            modelBuilder.Entity<Paciente>().ToTable("t_paciente");
            modelBuilder.Entity<Rol>().ToTable("t_rol");
            modelBuilder.Entity<Usuario>().ToTable("t_usuario");
            modelBuilder.Entity<Menu>().ToTable("t_menu");
            modelBuilder.Entity<RolMenu>().ToTable("t_rol_menu");

            // RELACIONES EXISTENTES ----------------------------------
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol);

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Usuario)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Paciente>(p => p.IdUsuario);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Usuario)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.IdUsuario);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.IdPaciente);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Doctor)
                .WithMany(d => d.Citas)
                .HasForeignKey(c => c.IdDoctor);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Citas)
                .HasForeignKey(c => c.IdUsuario);

            modelBuilder.Entity<HistorialMedico>()
                .HasOne(h => h.Paciente)
                .WithOne(p => p.HistorialMedico)
                .HasForeignKey<HistorialMedico>(h => h.IdPaciente);

            modelBuilder.Entity<DoctorEspecializacion>()
                .HasOne(de => de.Doctor)
                .WithMany(d => d.DoctorEspecializaciones)
                .HasForeignKey(de => de.IdDoctor);

            modelBuilder.Entity<DoctorEspecializacion>()
                .HasOne(de => de.Especializacion)
                .WithMany(e => e.DoctorEspecializaciones)
                .HasForeignKey(de => de.IdEspecializacion);

            // NUEVAS RELACIONES --------------------------------------
            modelBuilder.Entity<RolMenu>()
                .HasOne(rm => rm.Rol)
                .WithMany(r => r.RolMenus)
                .HasForeignKey(rm => rm.IdRol)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolMenu>()
                .HasOne(rm => rm.Menu)
                .WithMany(m => m.RolMenus)
                .HasForeignKey(rm => rm.IdMenu)
                .OnDelete(DeleteBehavior.Cascade);

            // UNIQUE para evitar duplicados rol-menu
            modelBuilder.Entity<RolMenu>()
                .HasIndex(rm => new { rm.IdRol, rm.IdMenu })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
