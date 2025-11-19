using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        // ✅ IMPLEMENTACIÓN DE MÉTODOS PRINCIPALES (devuelven DTOs)

        public async Task<RolResponseDTO> CrearRolConPermisosAsync(RolCreateUpdateDTO dto)
        {
            try
            {
                var roles = await _repository.GetRolesAsync();
                if (roles.Any(p => p.Nombre.ToLower() == dto.Nombre.ToLower()))
                    throw new InvalidOperationException("Ya existe un rol con el mismo nombre");

                var nuevoRol = new Rol { Nombre = dto.Nombre.Trim(), Estado = dto.Estado };
                var rolInsertado = await _repository.AddRolAsync(nuevoRol);

                // Crear permisos
                var allMenus = await _repository.GetAllMenusAsync();
                var rolMenus = allMenus.Select(m => new RolMenu
                {
                    IdRol = rolInsertado.IdRol,
                    IdMenu = m.IdMenu,
                    Habilitado = dto.Permisos?.FirstOrDefault(p => p.MenuId == m.IdMenu)?.Habilitado ?? false
                }).ToList();

                if (rolMenus.Any())
                    await _repository.AddRolMenusAsync(rolMenus);

                return await MapToRolResponseDTO(rolInsertado);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error creando rol: {ex.Message}", ex);
            }
        }

        public async Task<RolResponseDTO> ActualizarRolConPermisosAsync(int idRol, RolCreateUpdateDTO dto)
        {
            try
            {
                var rol = await _repository.GetRolByIdAsync(idRol);
                if (rol == null)
                    throw new KeyNotFoundException("Rol no encontrado");

                rol.Nombre = dto.Nombre.Trim();
                rol.Estado = dto.Estado;
                await _repository.UpdateRolAsync(rol);

                await ActualizarPermisosRol(idRol, dto.Permisos);

                return await MapToRolResponseDTO(rol);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error actualizando rol: {ex.Message}", ex);
            }
        }

        public async Task<RolResponseDTO> GetRolWithPermissionsAsync(int idRol)
        {
            var rol = await _repository.GetRolByIdAsync(idRol);
            if (rol == null) return null;

            return await MapToRolResponseDTO(rol);
        }

        public async Task<IEnumerable<RolResponseDTO>> GetAllRolesWithPermissionsAsync()
        {
            var roles = await _repository.GetRolesAsync();
            var result = new List<RolResponseDTO>();

            foreach (var rol in roles)
            {
                result.Add(await MapToRolResponseDTO(rol));
            }

            return result;
        }

        public async Task<IEnumerable<RolMenuDTO>> GetMenusPorRolAsync(int idRol)
        {
            var rolMenus = await _repository.GetRolMenusByRolIdAsync(idRol);
            var menus = await _repository.GetAllMenusAsync();

            return menus.Select(menu => new RolMenuDTO
            {
                MenuId = menu.IdMenu,
                NombreMenu = menu.Nombre,
                Habilitado = rolMenus.FirstOrDefault(rm => rm.IdMenu == menu.IdMenu)?.Habilitado ?? false
            }).ToList();
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _repository.GetAllMenusAsync();
        }

        public async Task<bool> TogglePermisoAsync(int idRol, int idMenu, bool habilitado)
        {
            try
            {
                var exist = await _repository.GetRolMenuAsync(idRol, idMenu);
                if (exist != null)
                {
                    exist.Habilitado = habilitado;
                    await _repository.UpdateRolMenusAsync(new List<RolMenu> { exist });
                }
                else
                {
                    var newRm = new RolMenu { IdRol = idRol, IdMenu = idMenu, Habilitado = habilitado };
                    await _repository.AddRolMenusAsync(new List<RolMenu> { newRm });
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DesactivarRolAsync(int id)
        {
            var rol = await _repository.GetRolByIdAsync(id);
            if (rol == null) return false;

            rol.Estado = 0;
            await _repository.UpdateRolAsync(rol);
            return true;
        }

        public async Task<bool> ReactivarRolAsync(int id)
        {
            var rol = await _repository.GetRolByIdAsync(id);
            if (rol == null) return false;

            rol.Estado = 1;
            await _repository.UpdateRolAsync(rol);
            return true;
        }

        // ✅ MÉTODOS LEGACY (devuelven string) - RENOMBRADOS

        public async Task<string> AgregarRolConPermisosAsync(RolCreateUpdateDTO dto)
        {
            try
            {
                await CrearRolConPermisosAsync(dto);
                return "Rol agregado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // Renombrado para evitar conflicto
        public async Task<string> ActualizarRolConPermisosLegacyAsync(int idRol, RolCreateUpdateDTO dto)
        {
            try
            {
                await ActualizarRolConPermisosAsync(idRol, dto);
                return "Rol actualizado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> TogglePermisoLegacyAsync(int idRol, int idMenu, bool habilitado)
        {
            try
            {
                var resultado = await TogglePermisoAsync(idRol, idMenu, habilitado);
                return resultado ? "Permiso actualizado" : "Error al actualizar permiso";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // ✅ MÉTODOS EXISTENTES QUE YA TENÍAS

        public async Task<Rol?> ObtenerRolPorIdAsync(int id)
        {
            if (id <= 0) return null;
            var rol = await _repository.GetRolByIdAsync(id);
            if (rol != null && rol.Estado == 1) return rol;
            return null;
        }

        public async Task<IEnumerable<Rol>> ObtenerRolesActivosAsync()
        {
            var roles = await _repository.GetRolesAsync();
            return roles.Where(r => r.Estado == 1);
        }

        public async Task<string> DesactivarRolPorIdAsync(int id)
        {
            var resultado = await DesactivarRolAsync(id);
            return resultado ? "Rol desactivado correctamente" : "Error: Rol no encontrado";
        }

        public async Task<string> ReactivarRolPorIdAsync(int id)
        {
            var resultado = await ReactivarRolAsync(id);
            return resultado ? "Rol reactivado correctamente" : "Error: Rol no encontrado";
        }

        // ✅ MÉTODOS PRIVADOS

        private async Task<RolResponseDTO> MapToRolResponseDTO(Rol rol)
        {
            var permisos = await _repository.GetRolMenusByRolIdAsync(rol.IdRol);
            var menus = await _repository.GetAllMenusAsync();

            return new RolResponseDTO
            {
                IdRol = rol.IdRol,
                Nombre = rol.Nombre,
                Estado = rol.Estado,
                Permisos = menus.Select(menu => new RolMenuDTO
                {
                    MenuId = menu.IdMenu,
                    NombreMenu = menu.Nombre,
                    Habilitado = permisos.FirstOrDefault(p => p.IdMenu == menu.IdMenu)?.Habilitado ?? false
                }).ToList()
            };
        }

        private async Task ActualizarPermisosRol(int idRol, List<RolMenuDTO> permisosDto)
        {
            var existing = (await _repository.GetRolMenusByRolIdAsync(idRol)).ToList();
            var menus = (await _repository.GetAllMenusAsync()).ToList();

            var updateList = new List<RolMenu>();
            var addList = new List<RolMenu>();

            foreach (var menu in menus)
            {
                var dtoPerm = permisosDto.FirstOrDefault(p => p.MenuId == menu.IdMenu);
                var exist = existing.FirstOrDefault(e => e.IdMenu == menu.IdMenu);
                var habilitado = dtoPerm?.Habilitado ?? false;

                if (exist != null)
                {
                    exist.Habilitado = habilitado;
                    updateList.Add(exist);
                }
                else
                {
                    addList.Add(new RolMenu { IdRol = idRol, IdMenu = menu.IdMenu, Habilitado = habilitado });
                }
            }

            if (updateList.Any())
                await _repository.UpdateRolMenusAsync(updateList);
            if (addList.Any())
                await _repository.AddRolMenusAsync(addList);
        }
    }
}