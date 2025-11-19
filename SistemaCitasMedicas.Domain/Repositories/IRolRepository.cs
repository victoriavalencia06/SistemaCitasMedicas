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
        // Obtiene todos los menús (t_menu)
        Task<IEnumerable<Menu>> GetAllMenusAsync();

        // Obtiene las filas t_rol_menu para un rol
        Task<IEnumerable<RolMenu>> GetRolMenusByRolIdAsync(int idRol);

        // Inserta una lista de RolMenu
        Task AddRolMenusAsync(IEnumerable<RolMenu> rolMenus);

        // Actualiza una lista de RolMenu (puede ser Update por cada item dentro)
        Task UpdateRolMenusAsync(IEnumerable<RolMenu> rolMenus);

        // Opcional: obtener un RolMenu por rol+menu
        Task<RolMenu?> GetRolMenuAsync(int idRol, int idMenu);

    }
}
