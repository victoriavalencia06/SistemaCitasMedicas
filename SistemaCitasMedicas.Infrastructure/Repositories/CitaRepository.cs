using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System;
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
                    .ThenInclude(p => p.Usuario) // si el nombre está en Usuario
                .Include(c => c.Doctor)
                    .ThenInclude(d => d.Usuario) // si el nombre está en Usuario
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
    }
}
