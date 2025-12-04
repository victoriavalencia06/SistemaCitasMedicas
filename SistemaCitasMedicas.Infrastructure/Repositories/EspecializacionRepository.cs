using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class EspecializacionRepository : IEspecializacionRepository
    {
        private readonly AppDBContext _context;

        public EspecializacionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especializacion>> GetEspecializacionesAsync()
        {
            return await _context.Especializaciones.ToListAsync();
        }

        public async Task<Especializacion> GetEspecializacionByIdAsync(int id)
        {
            return await _context.Especializaciones.FindAsync(id);
        }

        public async Task<Especializacion> AddEspecializacionAsync(Especializacion especializacion)
        {
            _context.Especializaciones.Add(especializacion);
            await _context.SaveChangesAsync();
            return especializacion;
        }

        public async Task<Especializacion> UpdateEspecializacionAsync(Especializacion especializacion)
        {
            _context.Entry(especializacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return especializacion;
        }

        public async Task<bool> DeleteEspecializacionAsync(int id)
        {
            var especializacion = await _context.Especializaciones.FindAsync(id);
            if (especializacion == null) return false;

            _context.Especializaciones.Remove(especializacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<object>> GetDoctoresPorEspecializacionAsync()
        {
            return await _context.Especializaciones
                .Where(e => e.Estado == 1)
                .Select(e => new
                {
                    EspecializacionId = e.IdEspecializacion,
                    EspecializacionNombre = e.Nombre,
                    CantidadDoctores = e.DoctorEspecializaciones
                        .Count(de => de.Doctor.Estado == 1)
                })
                .OrderByDescending(x => x.CantidadDoctores)
                .ToListAsync();
        }

        public async Task<object> GetEspecializacionConMasDoctoresAsync()
        {
            return await _context.Especializaciones
                .Where(e => e.Estado == 1)
                .Select(e => new
                {
                    EspecializacionId = e.IdEspecializacion,
                    EspecializacionNombre = e.Nombre,
                    CantidadDoctores = e.DoctorEspecializaciones
                        .Count(de => de.Doctor.Estado == 1)
                })
                .OrderByDescending(x => x.CantidadDoctores)
                .FirstOrDefaultAsync();
        }
    }
}
