using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDBContext _context;

        public DoctorRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetDoctoresAsync()
        {
            return await _context.Doctores.ToListAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctores.FindAsync(id);
        }

        public async Task<Doctor> AddDoctorAsync(Doctor doctor)
        {
            _context.Doctores.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor> UpdateDoctorAsync(Doctor doctor)
        {
            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctores.FindAsync(id);
            if (doctor == null) return false;

            _context.Doctores.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
