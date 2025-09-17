using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
