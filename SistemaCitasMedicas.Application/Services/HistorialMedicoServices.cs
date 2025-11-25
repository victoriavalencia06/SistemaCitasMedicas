using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public class HistorialMedicoServices
    {
        private readonly IHistorialMedicoRepository _repository;

        public HistorialMedicoServices(IHistorialMedicoRepository repository)
        {
            _repository = repository;
        }

        public async Task<HistorialMedico?> ObtenerHistorialMedicoPorIdAsync(int id)
        {
            if (id <= 0) return null;

            var historial = await _repository.GetHistorialMedicoByIdAsync(id);
            return historial?.Estado == 1 ? historial : null;
        }

        public async Task<IEnumerable<HistorialMedico>> ObtenerHistorialesMedicosActivosAsync()
        {
            var historiales = await _repository.GetHistorialesMedicosAsync();
            return historiales.Where(x => x.Estado == 1);
        }

        // 👉 Agregar historial con validación de duplicado
        public async Task<string> AgregarHistorialMedicoAsync(HistorialMedico nuevoHistorial)
        {
            try
            {
                if (nuevoHistorial.IdPaciente <= 0)
                    return "Error: el historial debe pertenecer a un paciente válido.";

                if (nuevoHistorial.IdCita <= 0)
                    return "Error: el historial debe estar asociado a una cita válida.";

                if (string.IsNullOrWhiteSpace(nuevoHistorial.Diagnostico))
                    return "Error: el diagnóstico es requerido.";

                // Validar duplicado
                var existe = await _repository.ExisteHistorialDuplicadoAsync(
                    nuevoHistorial.IdPaciente,
                    nuevoHistorial.FechaHora
                );

                if (existe)
                    return "Error: ya existe un historial registrado para este paciente en esa fecha.";

                nuevoHistorial.Estado = 1;

                await _repository.AddHistorialMedicoAsync(nuevoHistorial);
                return "Historial médico agregado correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }

        public async Task<string> ModificarHistorialMedicoAsync(HistorialMedico historial)
        {
            var existente = await _repository.GetHistorialMedicoByIdAsync(historial.IdHistorialMedico);
            if (existente == null)
                return "Error: Historial no encontrado.";

            // ACTUALIZAR TODOS LOS CAMPOS NUEVOS
            existente.Notas = historial.Notas;
            existente.Diagnostico = historial.Diagnostico;
            existente.Tratamientos = historial.Tratamientos;
            existente.CuadroMedico = historial.CuadroMedico;
            existente.Alergias = historial.Alergias;
            existente.AntecedentesFamiliares = historial.AntecedentesFamiliares;
            existente.Observaciones = historial.Observaciones;
            existente.FechaHora = historial.FechaHora;

            await _repository.UpdateHistorialMedicoAsync(existente);
            return "Historial médico modificado correctamente.";
        }

        public async Task<string> DesactivarHistorialMedicoAsync(int id)
        {
            var ok = await _repository.DesactivarHistorialMedicoAsync(id);
            return ok ? "Historial médico desactivado exitosamente." :
                        "Error: el historial no existe o ya está inactivo.";
        }

        // 👉 NUEVO: Obtener historiales por paciente
        public async Task<IEnumerable<HistorialMedico>> ObtenerHistorialesPorPacienteAsync(int idPaciente)
        {
            var historiales = await _repository.GetHistorialesMedicosAsync();
            return historiales.Where(x => x.IdPaciente == idPaciente && x.Estado == 1);
        }

        // 👉 NUEVO: Obtener historial por cita
        public async Task<HistorialMedico?> ObtenerHistorialPorCitaAsync(int idCita)
        {
            var historiales = await _repository.GetHistorialesMedicosAsync();
            return historiales.FirstOrDefault(x => x.IdCita == idCita && x.Estado == 1);
        }
    }
}