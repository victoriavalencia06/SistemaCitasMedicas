using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class DoctorEspecializacionRepository : IDoctorEspecializacionRepository
    {
        private readonly AppDBContext _context;

        public DoctorEspecializacionRepository(AppDBContext context) => _context = context;

        public async Task<IEnumerable<DoctorEspecializacion>> GetEspecializacionesPorDoctorAsync(int doctorId) =>
            await _context.DoctorEspecializaciones
                          .Where(de => de.IdDoctor == doctorId)
                          .Include(de => de.Especializacion)
                          .ToListAsync();

        public async Task<IEnumerable<DoctorEspecializacion>> GetDoctoresPorEspecializacionAsync(int especializacionId) =>
            await _context.DoctorEspecializaciones
                          .Where(de => de.IdEspecializacion == especializacionId)
                          .Include(de => de.Doctor)
                          .ToListAsync();

        public async Task<DoctorEspecializacion> AddDoctorEspecializacionAsync(DoctorEspecializacion relacion)
        {
            _context.DoctorEspecializaciones.Add(relacion);
            await _context.SaveChangesAsync();
            return relacion;
        }

        public async Task<bool> DeleteDoctorEspecializacionAsync(int doctorId, int especializacionId)
        {
            var relacion = await _context.DoctorEspecializaciones
                                         .FirstOrDefaultAsync(de => de.IdDoctor == doctorId
                                                                 && de.IdEspecializacion == especializacionId);

            return relacion != null && (_context.DoctorEspecializaciones.Remove(relacion) != null
                                         && await _context.SaveChangesAsync() > 0);
        }
    }
}
