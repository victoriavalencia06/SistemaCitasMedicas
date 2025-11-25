using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
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

        public async Task<HistorialMedico?> GetHistorialMedicoByIdAsync(int id)
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
            _context.HistorialesMedicos.Update(historialMedico);
            await _context.SaveChangesAsync();
            return historialMedico;
        }

        public async Task<bool> DesactivarHistorialMedicoAsync(int id)
        {
            var historial = await _context.HistorialesMedicos.FindAsync(id);
            if (historial == null || historial.Estado == 0)
                return false;

            historial.Estado = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        // Validar duplicado por paciente + fecha (usado por el servicio)
        public async Task<bool> ExisteHistorialDuplicadoAsync(int idPaciente, DateTime fecha)
        {
            return await _context.HistorialesMedicos
                .AnyAsync(h =>
                    h.IdPaciente == idPaciente &&
                    h.FechaHora.Date == fecha.Date &&
                    h.Estado == 1);
        }

        // NUEVO: Obtener historiales por paciente
        public async Task<IEnumerable<HistorialMedico>> GetHistorialesByPacienteAsync(int idPaciente)
        {
            return await _context.HistorialesMedicos
                .Where(h => h.IdPaciente == idPaciente && h.Estado == 1)
                .ToListAsync();
        }

        // NUEVO: Obtener historial por cita
        public async Task<HistorialMedico?> GetHistorialByCitaAsync(int idCita)
        {
            return await _context.HistorialesMedicos
                .FirstOrDefaultAsync(h => h.IdCita == idCita && h.Estado == 1);
        }
    }
}