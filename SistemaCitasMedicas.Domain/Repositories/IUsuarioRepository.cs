using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        // Obtener todos los usuarios
        Task<IEnumerable<Usuario>> GetUsuariosAsync();

        // Obtener un usuario por su id
        Task<Usuario> GetUsuarioByIdAsync(long id);

        // Agregar un nuevo usuario
        Task<Usuario> AddUsuarioAsync(Usuario usuario);

        // Actualizar un usuario existente
        Task<Usuario> UpdateUsuarioAsync(Usuario usuario);

        // Eliminar un usuario por su id
        Task<bool> DeleteUsuarioAsync(long id);

        // Verificar si el correo ya existe (requerido por AuthService)
        Task<Usuario> GetByCorreoAsync(string correo);
    }
}
