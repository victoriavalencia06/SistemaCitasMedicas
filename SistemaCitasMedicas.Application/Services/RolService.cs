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
            var rol = await _repository.GetRolesAsync();
            return rol.Where(p => p.Estado == 1);
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


                return "Rol agregado correctamnete";

            }
            catch (Exception ex)
            {

                return $"Error de servidor: {ex.Message}";
            }

        }

       
    }
}
