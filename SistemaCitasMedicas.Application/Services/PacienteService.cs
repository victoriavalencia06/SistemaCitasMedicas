using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Paciente>> ObtenerTodosPacientesAsync()
        {
            return await _repository.GetPacientesAsync();
        }

        public async Task<Paciente?> ObtenerPacientePorIdAsync(int idPaciente)
        {
            var paciente = await _repository.GetPacienteByIdAsync(idPaciente);

            return (paciente != null && paciente.Estado == 1)
                ? paciente
                : null;
        }

        public async Task<string> AgregarPacienteAsync(Paciente nuevoPaciente)
        {
            if (await _repository.ExistePacienteDuplicadoAsync(nuevoPaciente))
                return "Error: Ya existe un paciente con los mismos datos.";

            nuevoPaciente.Estado = 1;

            var resultado = await _repository.AddPacienteAsync(nuevoPaciente);

            return resultado.IdPaciente > 0
                ? "Paciente agregado correctamente."
                : "Error: No se pudo registrar el paciente.";
        }

        public async Task<string> ModificarPacienteAsync(Paciente paciente)
        {
            var existente = await _repository.GetPacienteByIdAsync(paciente.IdPaciente);

            if (existente == null)
                return "Error: Paciente no encontrado.";

            // Actualizar campos
            existente.IdUsuario = paciente.IdUsuario;
            existente.Nombre = paciente.Nombre;
            existente.Apellido = paciente.Apellido;
            existente.FechaNacimiento = paciente.FechaNacimiento;
            existente.Telefono = paciente.Telefono;
            existente.Direccion = paciente.Direccion;
            existente.Seguro = paciente.Seguro;
            existente.Estado = paciente.Estado;

            await _repository.UpdatePacienteAsync(existente);

            return "Paciente modificado correctamente.";
        }

        public async Task<string> DesactivaPacientePorIdAsync(int id)
        {
            var paciente = await _repository.GetPacienteByIdAsync(id);

            if (paciente == null)
                return "Error: Paciente no encontrado.";

            if (paciente.Estado == 0)
                return "Error: El paciente ya está inactivo.";

            paciente.Estado = 0;

            await _repository.UpdatePacienteAsync(paciente);

            return "Paciente desactivado exitosamente.";
        }

        public async Task<int> GetTotalPatientsAsync()
        {
            return await _repository.GetTotalPatientsAsync();
        }
    }
}
