using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        // Baja lógica: cambia estado a 0
        Task<bool> DesactivarHistorialMedicoAsync(int id);
    }
}
