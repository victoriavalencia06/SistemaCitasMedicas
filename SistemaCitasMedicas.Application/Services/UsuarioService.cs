using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SistemaCitasMedicas.Application.Services
{
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
            if (id <= 0) return null;

            var usuario = await _repository.GetUsuarioByIdAsync(id);
            if (usuario != null && usuario.Estado == 1) return usuario;

            return null;
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
            existente.Estado = usuario.Estado;

            // ENCRIPTAR LA NUEVA CONTRASEÑA SI SE PROPORCIONA
            if (!string.IsNullOrEmpty(usuario.Password))
            {
                existente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            }

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

                // ENCRIPTAR LA CONTRASEÑA ANTES DE GUARDAR
                if (!string.IsNullOrEmpty(nuevoUsuario.Password))
                {
                    nuevoUsuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Password);
                    nuevoUsuario.Password = null; // Limpiar el campo de texto plano
                }
                else
                {
                    return "Error: La contraseña es requerida";
                }

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
            var usuario = await _repository.GetUsuarioByIdAsync(id);
            if (usuario == null || usuario.Estado == 0)
                return "Error: el usuario no existe o ya está inactivo.";

            usuario.Estado = 0;
            await _repository.UpdateUsuarioAsync(usuario);
            return "Usuario desactivado exitosamente.";
        }

        // Método adicional: Reactivar usuario
        public async Task<string> ReactivarUsuarioPorIdAsync(int id)
        {
            var usuario = await _repository.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return "Error: el usuario no existe.";

            usuario.Estado = 1;
            await _repository.UpdateUsuarioAsync(usuario);
            return "Usuario reactivado exitosamente.";
        }

        // Método adicional: Cambiar contraseña
        public async Task<string> CambiarContraseñaAsync(int idUsuario, string nuevaPassword)
        {
            if (string.IsNullOrEmpty(nuevaPassword) || nuevaPassword.Length < 6)
                return "Error: La contraseña debe tener al menos 6 caracteres";

            var usuario = await _repository.GetUsuarioByIdAsync(idUsuario);
            if (usuario == null)
                return "Error: Usuario no encontrado";

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);
            await _repository.UpdateUsuarioAsync(usuario);

            return "Contraseña cambiada exitosamente";
        }
    }
}