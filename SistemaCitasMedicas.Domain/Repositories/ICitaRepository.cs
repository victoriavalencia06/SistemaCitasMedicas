using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface ICitaRepository
    {
        // Obtener todas las citas con relaciones
        Task<IEnumerable<Cita>> GetCitasAsync();

        // Obtener una cita por su id
        Task<Cita> GetCitaByIdAsync(int id);

        // Agregar una nueva cita
        Task<Cita> AddCitaAsync(Cita cita);

        // Actualizar una cita existente
        Task<Cita> UpdateCitaAsync(Cita cita);

        // Eliminar una cita por su id
        Task<bool> DeleteCitaAsync(int id);

        // Validar si una cita ya existe (paciente + fecha/hora)
        Task<bool> ExisteCitaDuplicadaAsync(Cita cita);
    }
}
