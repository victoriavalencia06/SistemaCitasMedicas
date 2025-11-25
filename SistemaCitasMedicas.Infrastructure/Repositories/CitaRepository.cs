using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly AppDBContext _context;

        public CitaRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> GetCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Doctor)
                .ToListAsync();
        }

        public async Task<Cita> GetCitaByIdAsync(int id)
        {
            return await _context.Citas.FindAsync(id);
        }

        public async Task<Cita> AddCitaAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task<Cita> UpdateCitaAsync(Cita cita)
        {
            _context.Entry(cita).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task<bool> DeleteCitaAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        // Validar si ya existe una cita para un paciente en la misma fecha y hora
        public async Task<bool> ExisteCitaDuplicadaAsync(Cita cita)
        {
            return await _context.Citas.AnyAsync(c =>
                c.IdPaciente == cita.IdPaciente &&
                c.FechaHora == cita.FechaHora &&
                c.IdCita != cita.IdCita // permite actualizar citas sin caer en falso duplicado
            );
        }
    }
}
