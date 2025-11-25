using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IPacienteRepository
    {
        // Obtener todos los pacientes
        Task<IEnumerable<Paciente>> GetPacientesAsync();

        // Obtener un paciente por su id
        Task<Paciente> GetPacienteByIdAsync(int id);

        // Agregar un nuevo paciente
        Task<Paciente> AddPacienteAsync(Paciente paciente);

        // Actualizar un paciente existente
        Task<Paciente> UpdatePacienteAsync(Paciente paciente);

        // Eliminar un paciente por su id
        Task<bool> DeletePacienteAsync(int id);

        // 🔍 NUEVO: Validar si existe paciente duplicado
        Task<bool> ExistePacienteDuplicadoAsync(Paciente paciente);
    }
}
