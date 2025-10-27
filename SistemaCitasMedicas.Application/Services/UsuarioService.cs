using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;

namespace SistemaCitasMedicas.Application.Services
{
    // Algoritmos con lógica de negocio (UseCase)
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        // Caso de uso: Buscar un usuario por Id (solo activos)
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(long id)
        {
            if (id <= 0) return null; // Id no válido

            var usuario = await _repository.GetUsuarioByIdAsync(id);
            if (usuario != null && usuario.Estado == 1) return usuario;

            return null; // No encontrado o inactivo
        }

        // Caso de uso: Modificar un usuario
        public async Task<string> ModificarUsuarioAsync(Usuario usuario)
        {
            if (usuario.IdUsuario <= 0) return "Error: Id no válido";

            var existente = await _repository.GetUsuarioByIdAsync(usuario.IdUsuario);
            if (existente == null) return "Error: Usuario no encontrado";

            // Actualizar propiedades
            existente.Nombre = usuario.Nombre;
            existente.Correo = usuario.Correo;
            existente.IdRol = usuario.IdRol;
            existente.Password = usuario.PasswordHash;
            existente.Estado = usuario.Estado; // Permitir cambiar estado

            await _repository.UpdateUsuarioAsync(existente);

            return "Usuario modificado correctamente";
        }

        // Caso de uso: Obtener solo usuarios activos
        public async Task<IEnumerable<Usuario>> ObtenerUsuariosActivosAsync()
        {
            var usuarios = await _repository.GetUsuariosAsync();
            return usuarios.Where(u => u.Estado == 1);
        }

        // Caso de uso: Agregar usuario (validar duplicado)
        public async Task<string> AgregarUsuarioAsync(Usuario nuevoUsuario)
        {
            try
            {
                var usuarios = await _repository.GetUsuariosAsync();

                if (usuarios.Any(u => u.Correo.ToLower() == nuevoUsuario.Correo.ToLower()))
                    return "Error: ya existe un usuario con el mismo correo";

                nuevoUsuario.Estado = 1; // Activo por defecto
                var usuarioInsertado = await _repository.AddUsuarioAsync(nuevoUsuario);

                if (usuarioInsertado == null || usuarioInsertado.IdUsuario <= 0)
                    return "Error: no se pudo agregar el usuario";

                return "Usuario agregado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }

        // Cambia estado a inactivo en lugar de eliminar
        public async Task<string> DesactivarUsuarioPorIdAsync(int id)
        {
            var doctor = await _repository.GetUsuarioByIdAsync(id);
            if (doctor == null || doctor.Estado == 0) return "Error: el usuario no existe o ya está inactivo.";

            doctor.Estado = 0;
            await _repository.UpdateUsuarioAsync(doctor);
            return "Usuario desactivado exitosamente.";
        }
    }
}
