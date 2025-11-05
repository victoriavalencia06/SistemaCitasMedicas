using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;

namespace SistemaCitasMedicas.Application.Services
{
    // Algoritmos con logica de negocio (UsseCase)
    public class RolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }
        // Caso de uso: Buscar un producto po ru Id (solo activos)
        public async Task<Rol?> ObtenerRolPorIdAsync(int id)
        {
            if (id <= 0)
                return null;//Id no valdo

            var rol = await _repository.GetRolByIdAsync(id);

            if (rol != null && rol.Estado == 1)
                return rol;
            return null; //No encontrdo o inactivo
        }

        // Caso uso: Modificar un rol
        public async Task<string> ModificarRolAsync(Rol rol)
        {
            if (rol.IdRol <= 0)
                return "Error; Id no valido";//Id no valdo

            var existente = await _repository.GetRolByIdAsync(rol.IdRol);
            if (existente == null)
                return "Error: Rol no encontrado";
            existente.Nombre = rol.Nombre;
            existente.Estado = rol.Estado;//Permitir cambiar estado

            await _repository.UpdateRolAsync(existente);

            return "Rol modificado correctamente";
        }

        // Caso de uso: Obtener solo roles activos
        public async Task<IEnumerable<Rol>> ObtenerRolesActivosAsync()
        {
            return await _repository.GetRolesAsync();
        }

        // Caso de uso: Agregar rol (validar duplicado)
        public async Task<string> AgregarRolAsync(Rol NuevoRol)
        {
            try
            {
                var roles = await _repository.GetRolesAsync();

                if (roles.Any(p => p.Nombre.ToLower() == NuevoRol.Nombre.ToLower()))

                    return "Error: ya existe un rol con el mismo nombre";
                NuevoRol.Estado = 1;//Activo por defecto
                var rolinsertado = await _repository.AddRolAsync(NuevoRol);

                if (rolinsertado == null || rolinsertado.IdRol <= 0)
                    return "Error: no se pudo agreagar el rol";


                return "Rol agregado correctamente";

            }
            catch (Exception ex)
            {

                return $"Error de servidor: {ex.Message}";
            }

        }

        // Cambia estado a inactivo en lugar de eliminar
        public async Task<string> DesactivarRolPorIdAsync(int id)
        {
            var rol = await _repository.GetRolByIdAsync(id);
            if (rol == null || rol.Estado == 0) return "Error: el rol no existe o ya está inactivo.";

            rol.Estado = 0;
            await _repository.UpdateRolAsync(rol);
            return "Rol desactivado exitosamente.";
        }

        public async Task<string> ReactivarRolPorIdAsync(int id)
        {
            if (id <= 0) return "Error: Id no válido.";

            var rol = await _repository.GetRolByIdAsync(id);
            if (rol == null) return "Error: el rol no existe.";

            if (rol.Estado == 1) return "Error: el rol ya está activo.";

            rol.Estado = 1;
            var rolActualizado = await _repository.UpdateRolAsync(rol);

            if (rolActualizado == null) return "Error al reactivar el rol.";

            return "Rol reactivado exitosamente.";
        }
    }
}
