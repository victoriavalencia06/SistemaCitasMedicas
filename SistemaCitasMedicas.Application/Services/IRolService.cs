using SistemaCitasMedicas.Application.DTOs;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public interface IRolService
    {
        // Métodos principales que devuelven DTOs
        Task<RolResponseDTO> CrearRolConPermisosAsync(RolCreateUpdateDTO dto);
        Task<RolResponseDTO> ActualizarRolConPermisosAsync(int idRol, RolCreateUpdateDTO dto);
        Task<RolResponseDTO> GetRolWithPermissionsAsync(int idRol);
        Task<IEnumerable<RolResponseDTO>> GetAllRolesWithPermissionsAsync();
        Task<IEnumerable<RolMenuDTO>> GetMenusPorRolAsync(int idRol);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<bool> TogglePermisoAsync(int idRol, int idMenu, bool habilitado);
        Task<bool> DesactivarRolAsync(int id);
        Task<bool> ReactivarRolAsync(int id);

        // Métodos legacy (los que ya tenías) - RENOMBRADOS para evitar conflicto
        Task<string> AgregarRolConPermisosAsync(RolCreateUpdateDTO dto);
        Task<string> ActualizarRolConPermisosLegacyAsync(int idRol, RolCreateUpdateDTO dto);
        Task<Rol?> ObtenerRolPorIdAsync(int id);
        Task<IEnumerable<Rol>> ObtenerRolesActivosAsync();
        Task<string> DesactivarRolPorIdAsync(int id);
        Task<string> ReactivarRolPorIdAsync(int id);
        Task<string> TogglePermisoLegacyAsync(int idRol, int idMenu, bool habilitado);
    }
}