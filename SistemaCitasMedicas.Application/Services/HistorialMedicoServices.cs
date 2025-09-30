using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;

namespace SistemaCitasMedicas.Application.Services
{
    // Lógica de negocio (UseCases)
    public class HistorialMedicoServices
    {
        private readonly IHistorialMedicoRepository _repository;

        public HistorialMedicoServices(IHistorialMedicoRepository repository)
        {
            _repository = repository;
        }

        // Caso de uso: Buscar un historial médico por Id (solo activos)
        public async Task<HistorialMedico?> ObtenerHistorialMedicoPorIdAsync(int id)
        {
            if (id <= 0) return null; // Id no válido

            var historial = await _repository.GetHistorialMedicoByIdAsync(id);
            if (historial != null && historial.Estado == 1) return historial;

            return null; // No encontrado o inactivo
        }

        // Caso de uso: Modificar un historial médico
        public async Task<string> ModificarHistorialMedicoAsync(HistorialMedico historial)
        {
            if (historial.IdHistorialMedico <= 0) return "Error: Id no válido";

            var existente = await _repository.GetHistorialMedicoByIdAsync(historial.IdHistorialMedico);
            if (existente == null) return "Error: Historial médico no encontrado";

            // Actualizar propiedades
            existente.Notas = historial.Notas;
            existente.Diagnostico = historial.Diagnostico;
            existente.Tratamientos = historial.Tratamientos;
            existente.CuadroMedico = historial.CuadroMedico;
            existente.FechaHora = historial.FechaHora;
            existente.Estado = historial.Estado; // Permitir cambiar estado

            await _repository.UpdateHistorialMedicoAsync(existente);

            return "Historial médico modificado correctamente";
        }

        // Caso de uso: Obtener solo historiales médicos activos
        public async Task<IEnumerable<HistorialMedico>> ObtenerHistorialesMedicosActivosAsync()
        {
            var historiales = await _repository.GetHistorialesMedicosAsync();
            return historiales.Where(h => h.Estado == 1);
        }

        // Caso de uso: Agregar historial médico (validar duplicado por diagnostico)
        public async Task<string> AgregarHistorialMedicoAsync(HistorialMedico nuevoHistorial)
        {
            try
            {
                var historiales = await _repository.GetHistorialesMedicosAsync();

                if (!string.IsNullOrEmpty(nuevoHistorial.Diagnostico) &&
                    historiales.Any(h => h.Diagnostico.ToLower() == nuevoHistorial.Diagnostico.ToLower()))
                    return "Error: ya existe un historial con el mismo diagnóstico";

                nuevoHistorial.Estado = 1; // Activo por defecto
                var insertado = await _repository.AddHistorialMedicoAsync(nuevoHistorial);

                if (insertado == null || insertado.IdHistorialMedico <= 0)
                    return "Error: no se pudo agregar el historial médico";

                return "Historial médico agregado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }

        // Caso de uso: Desactivar historial médico (soft delete)
        public async Task<string> DesactivarHistorialMedicoAsync(int id)
        {
            if (id <= 0) return "Error: Id no válido";

            var historial = await _repository.GetHistorialMedicoByIdAsync(id);
            if (historial == null || historial.Estado == 0)
                return "Error: el historial no existe o ya está inactivo.";

            historial.Estado = 0;
            await _repository.UpdateHistorialMedicoAsync(historial);

            return "Historial médico desactivado exitosamente.";
        }
    }
}
