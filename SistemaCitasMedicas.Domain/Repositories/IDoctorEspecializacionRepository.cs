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
        // Obtener todas las especializaciones asignadas a un doctor
        Task<IEnumerable<DoctorEspecializacion>> GetEspecializacionesPorDoctorAsync(int doctorId);

        // Obtener todos los doctores que tengan una especialización específica
        Task<IEnumerable<DoctorEspecializacion>> GetDoctoresPorEspecializacionAsync(int especializacionId);

        // Asignar una especialización a un doctor
        Task<DoctorEspecializacion> AddDoctorEspecializacionAsync(DoctorEspecializacion relacion);

        // Quitar una especialización de un doctor
        Task<bool> DeleteDoctorEspecializacionAsync(int doctorId, int especializacionId);
    }
}
