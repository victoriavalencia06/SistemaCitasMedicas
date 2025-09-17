using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IRolRepository
    {
        // Obtener todos los roles
        Task<IEnumerable<Rol>> GetRolesAsync();

        // Obtener un rol por su id
        Task<Rol> GetRolByIdAsync(int id);

        // Agregar un nuevo rol
        Task<Rol> AddRolAsync(Rol rol);

        // Actualizar un rol existente
        Task<Rol> UpdateRolAsync(Rol rol);

        // Eliminar un rol por su id
        Task<bool> DeleteRolAsync(int id);
    }
}
