using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository) =>
            _doctorRepository = doctorRepository;

        public async Task<IEnumerable<Doctor>> ObtenerTodosLosDoctoresAsync() =>
            (await _doctorRepository.GetDoctoresAsync()).Where(d => d.Estado == 1);

        public async Task<Doctor> ObtenerDoctorPorIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID debe ser mayor que 0.");
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null || doctor.Estado != 1)
                throw new KeyNotFoundException("Doctor no encontrado o inactivo.");
            return doctor;
        }

        public async Task<string> AgregarDoctorAsync(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.Nombre))
                return "Error: el nombre es obligatorio.";

            doctor.Estado = 1;
            var nuevo = await _doctorRepository.AddDoctorAsync(doctor);
            return nuevo.IdDoctor > 0 ? "Doctor agregado exitosamente." : "Error al insertar el doctor.";
        }

        public async Task<string> ModificarDoctorAsync(Doctor doctor)
        {
            var existente = await _doctorRepository.GetDoctorByIdAsync(doctor.IdDoctor);
            if (existente == null) return "Error: el doctor no existe.";

            existente.Nombre = doctor.Nombre;
            existente.Apellido = doctor.Apellido;
            existente.Estado = doctor.Estado;

            await _doctorRepository.UpdateDoctorAsync(existente);
            return "Doctor modificado exitosamente.";
        }

        // Cambia estado a inactivo en lugar de eliminar
        public async Task<string>DesactivarDoctorPorIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null || doctor.Estado == 0) return "Error: el doctor no existe o ya está inactivo.";

            doctor.Estado = 0;
            await _doctorRepository.UpdateDoctorAsync(doctor);
            return "Doctor desactivado exitosamente.";
        }
    }
}
