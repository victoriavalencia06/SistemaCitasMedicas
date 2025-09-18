using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class DoctorEspecializacionRepository : IDoctorEspecializacionRepository
    {
        private readonly AppDBContext _context;

        public DoctorEspecializacionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorEspecializacion>> GetDoctorEspecializacionesAsync()
        {
            return await _context.DoctorEspecializaciones.ToListAsync();
        }

        public async Task<DoctorEspecializacion> GetDoctorEspecializacionByIdAsync(int id)
        {
            return await _context.DoctorEspecializaciones.FindAsync(id);
        }

        public async Task<DoctorEspecializacion> AddDoctorEspecializacionAsync(DoctorEspecializacion doctorEspecializacion)
        {
            _context.DoctorEspecializaciones.Add(doctorEspecializacion);
            await _context.SaveChangesAsync();
            return doctorEspecializacion;
        }

        public async Task<DoctorEspecializacion> UpdateDoctorEspecializacionAsync(DoctorEspecializacion doctorEspecializacion)
        {
            _context.Entry(doctorEspecializacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return doctorEspecializacion;
        }

        public async Task<bool> DeleteDoctorEspecializacionAsync(int id)
        {
            var doctorEspecializacion = await _context.DoctorEspecializaciones.FindAsync(id);
            if (doctorEspecializacion == null) return false;

            _context.DoctorEspecializaciones.Remove(doctorEspecializacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
