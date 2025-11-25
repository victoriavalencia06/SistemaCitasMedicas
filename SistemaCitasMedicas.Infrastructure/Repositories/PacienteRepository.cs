using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDBContext _context;

        public PacienteRepository(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>s
        /// Obtiene todos los pacientes (sin tracking para mejor rendimiento).
        /// </summary>
        public async Task<IEnumerable<Paciente>> GetPacientesAsync()
        {
            return await _context.Pacientes
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un paciente por ID.
        /// </summary>
        public async Task<Paciente?> GetPacienteByIdAsync(int id)
        {
            return await _context.Pacientes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        /// <summary>
        /// Valida si existe un paciente con los mismos datos.
        /// </summary>
        public async Task<bool> ExistePacienteDuplicadoAsync(Paciente p)
        {
            return await _context.Pacientes.AnyAsync(x =>
                x.Nombre.ToLower() == p.Nombre.ToLower() &&
                x.Apellido.ToLower() == p.Apellido.ToLower() &&
                x.FechaNacimiento == p.FechaNacimiento
            );
        }

        /// <summary>
        /// Inserta un paciente.
        /// </summary>
        public async Task<Paciente> AddPacienteAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }

        /// <summary>
        /// Actualiza un paciente existente.
        /// </summary>
        public async Task<Paciente> UpdatePacienteAsync(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }

        /// <summary>
        /// Elimina físicamente un paciente (no recomendado).
        /// Usado solo si la app lo requiere.
        /// </summary>
        public async Task<bool> DeletePacienteAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return false;

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
