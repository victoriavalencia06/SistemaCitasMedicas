using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public class PacienteService
    {
        private readonly IPacienteRepository _repository;
        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        // 1. Obtener todos los pacientes (activos e inactivos)
        public async Task<IEnumerable<Paciente>> ObtenerTodosPacientesAsync()
        {
            return await _repository.GetPacientesAsync();
        }

        // 2. Obtener paciente por ID (solo activo)
        public async Task<Paciente?> ObtenerPacientePorIdAsync(int idPaciente)
        {
            if (idPaciente <= 0)
                return null; // ID inválido

            var paciente = await _repository.GetPacienteByIdAsync(idPaciente);

            if (paciente != null && paciente.Estado == 1) // 1 = Activo
                return paciente;

            return null; // No encontrado o inactivo
        }

        // 3. Agregar nuevo paciente (validar duplicados)
        public async Task<string> AgregarPacienteAsync(Paciente nuevoPaciente)
        {
            try
            {
                var pacientes = await _repository.GetPacientesAsync();

                if (pacientes.Any(p =>
                    p.Nombre.ToLower() == nuevoPaciente.Nombre.ToLower() &&
                    p.Apellido.ToLower() == nuevoPaciente.Apellido.ToLower() &&
                    p.FechaNacimiento == nuevoPaciente.FechaNacimiento))
                {
                    return "Error: Ya existe un paciente con los mismos datos";
                }

                nuevoPaciente.Estado = 1; // Activo por defecto

                var pacienteInsertado = await _repository.AddPacienteAsync(nuevoPaciente);

                if (pacienteInsertado == null || pacienteInsertado.IdPaciente <= 0)
                    return "Error: No se pudo agregar el paciente";

                return "Paciente agregado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }

        // 4. Modificar paciente existente
        public async Task<string> ModificarPacienteAsync(Paciente paciente)
        {
            if (paciente.IdPaciente <= 0)
                return "Error: Datos de paciente inválidos";

            var existente = await _repository.GetPacienteByIdAsync(paciente.IdPaciente);

            if (existente == null)
                return "Error: Paciente no encontrado";

            // Actualizamos campos
            existente.IdUsuario = paciente.IdUsuario;
            existente.Nombre = paciente.Nombre;
            existente.Apellido = paciente.Apellido;
            existente.FechaNacimiento = paciente.FechaNacimiento;
            existente.Telefono = paciente.Telefono;
            existente.Direccion = paciente.Direccion;
            existente.Seguro = paciente.Seguro;
            existente.Estado = paciente.Estado;

            await _repository.UpdatePacienteAsync(existente);

            return "Paciente modificado correctamente";
        }
    }
}
