using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IEspecializacionRepository
    {
        // Obtener todas las especializaciones
        Task<IEnumerable<Especializacion>> GetEspecializacionesAsync();

        // Obtener una especialización por su id
        Task<Especializacion> GetEspecializacionByIdAsync(int id);

        // Agregar una nueva especialización
        Task<Especializacion> AddEspecializacionAsync(Especializacion especializacion);

        // Actualizar una especialización existente
        Task<Especializacion> UpdateEspecializacionAsync(Especializacion especializacion);

        // Eliminar una especialización por su id
        Task<bool> DeleteEspecializacionAsync(int id);

        // Obtener cantidad de doctores por especialización
        Task<IEnumerable<object>> GetDoctoresPorEspecializacionAsync();

        // Obtener la especialización con más doctores
        Task<object> GetEspecializacionConMasDoctoresAsync();
    }
}
