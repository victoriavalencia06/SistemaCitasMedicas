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
        // Obtener todos los historiales médicos
        Task<IEnumerable<HistorialMedico>> GetHistorialesMedicosAsync();

        // Obtener un historial médico por su id
        Task<HistorialMedico> GetHistorialMedicoByIdAsync(int id);

        // Agregar un nuevo historial médico
        Task<HistorialMedico> AddHistorialMedicoAsync(HistorialMedico historialMedico);

        // Actualizar un historial médico existente
        Task<HistorialMedico> UpdateHistorialMedicoAsync(HistorialMedico historialMedico);

        // Eliminar un historial médico por su id
        Task<bool> DeleteHistorialMedicoAsync(int id);
    }
}
