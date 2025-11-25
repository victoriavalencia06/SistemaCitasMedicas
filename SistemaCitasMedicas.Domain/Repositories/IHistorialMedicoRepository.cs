using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IHistorialMedicoRepository
    {
        // Obtener todos
        Task<IEnumerable<HistorialMedico>> GetHistorialesMedicosAsync();

        // Obtener uno por Id
        Task<HistorialMedico?> GetHistorialMedicoByIdAsync(int id);

        // Insertar nuevo
        Task<HistorialMedico> AddHistorialMedicoAsync(HistorialMedico historialMedico);

        // Actualizar existente
        Task<HistorialMedico> UpdateHistorialMedicoAsync(HistorialMedico historialMedico);

        // Baja lógica
        Task<bool> DesactivarHistorialMedicoAsync(int id);

        // Validar si existe historial duplicado (por paciente + fecha)
        Task<bool> ExisteHistorialDuplicadoAsync(int idPaciente, DateTime fecha);

        Task<IEnumerable<HistorialMedico>> GetHistorialesByPacienteAsync(int idPaciente);
        Task<HistorialMedico?> GetHistorialByCitaAsync(int idCita);
    }
}
