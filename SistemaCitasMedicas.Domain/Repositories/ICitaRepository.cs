using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface ICitaRepository
    {
        // Obtener todas las citas
        Task<IEnumerable<Cita>> GetCitasAsync();

        // Obtener una cita por su id
        Task<Cita> GetCitaByIdAsync(int id);

        // Agregar una nueva cita
        Task<Cita> AddCitaAsync(Cita cita);

        // Actualizar una cita existente
        Task<Cita> UpdateCitaAsync(Cita cita);

        // Eliminar una cita por su id
        Task<bool> DeleteCitaAsync(int id);
    }
}
