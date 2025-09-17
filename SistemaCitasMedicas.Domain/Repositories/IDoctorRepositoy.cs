using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IDoctorRepository
    {
        // Obtener todos los doctores
        Task<IEnumerable<Doctor>> GetDoctoresAsync();

        // Obtener un doctor por su id
        Task<Doctor> GetDoctorByIdAsync(int id);

        // Agregar un nuevo doctor
        Task<Doctor> AddDoctorAsync(Doctor doctor);

        // Actualizar un doctor existente
        Task<Doctor> UpdateDoctorAsync(Doctor doctor);

        // Eliminar un doctor por su id
        Task<bool> DeleteDoctorAsync(int id);
    }
}
