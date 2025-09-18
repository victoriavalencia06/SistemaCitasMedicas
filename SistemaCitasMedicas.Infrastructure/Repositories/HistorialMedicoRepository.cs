using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class HistorialMedicoRepository : IHistorialMedicoRepository
    {
        private readonly AppDBContext _context;

        public HistorialMedicoRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialMedico>> GetHistorialesMedicosAsync()
        {
            return await _context.HistorialesMedicos.ToListAsync();
        }

        public async Task<HistorialMedico> GetHistorialMedicoByIdAsync(int id)
        {
            return await _context.HistorialesMedicos.FindAsync(id);
        }

        public async Task<HistorialMedico> AddHistorialMedicoAsync(HistorialMedico historialMedico)
        {
            _context.HistorialesMedicos.Add(historialMedico);
            await _context.SaveChangesAsync();
            return historialMedico;
        }

        public async Task<HistorialMedico> UpdateHistorialMedicoAsync(HistorialMedico historialMedico)
        {
            _context.Entry(historialMedico).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return historialMedico;
        }

        public async Task<bool> DeleteHistorialMedicoAsync(int id)
        {
            var historial = await _context.HistorialesMedicos.FindAsync(id);
            if (historial == null) return false;

            _context.HistorialesMedicos.Remove(historial);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
