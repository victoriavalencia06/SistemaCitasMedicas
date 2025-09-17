using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IDoctorEspecializacionRepository
    {
        // Obtener todas las especializaciones de doctores
        Task<IEnumerable<DoctorEspecializacion>> GetDoctorEspecializacionesAsync();

        // Obtener una especialización de doctor por su id
        Task<DoctorEspecializacion> GetDoctorEspecializacionByIdAsync(int id);

        // Agregar una nueva especialización de doctor
        Task<DoctorEspecializacion> AddDoctorEspecializacionAsync(DoctorEspecializacion doctorEspecializacion);

        // Actualizar una especialización de doctor existente
        Task<DoctorEspecializacion> UpdateDoctorEspecializacionAsync(DoctorEspecializacion doctorEspecializacion);

        // Eliminar una especialización de doctor por su id
        Task<bool> DeleteDoctorEspecializacionAsync(int id);
    }
}
