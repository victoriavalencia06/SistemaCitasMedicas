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

            base.OnModelCreating(modelBuilder);
        }
    }
}
