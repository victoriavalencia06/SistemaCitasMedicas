using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    // Algoritmos con lógica de negocio (UseCase)
    public class HistorialMedicoServices
    {
        private readonly IHistorialMedicoRepository _repository;

        public HistorialMedicoServices(IHistorialMedicoRepository repository)
        {
            _repository = repository;
        }

        // Caso de uso: Buscar un historial Medico por Id (solo activos)
        public async Task<HistorialMedico?> ObtenerHistorialMedicoPorIdAsync(int id)
        {
            if (id <= 0) return null; // Id no válido

            var historialmedico = await _repository.GetHistorialMedicoByIdAsync(id);
            if (historialmedico != null && historialmedico.Estado == 1) return historialmedico;

            return null; // No encontrado o inactivo
        }

        // Caso de uso: Modificar un historial medico
        public async Task<string> ModificarHistorialMedicoAsync(HistorialMedico historialmedico)
        {
            if (historialmedico.IdHistorialMedico <= 0) return "Error: Id no válido";

            var existente = await _repository.GetHistorialMedicoByIdAsync(historialmedico.IdHistorialMedico);
            if (existente == null) return "Error: Usuario no encontrado";

            // Actualizar propiedades
            existente.Paciente = historialmedico.Paciente;
            existente.Notas = historialmedico.Notas;
            existente.Diagnostico = historialmedico.Diagnostico;
            existente.Tratamientos = historialmedico.Tratamientos;
            existente.Tratamientos = historialmedico.Tratamientos;
            existente.CuadroMedico = historialmedico.CuadroMedico;
            existente.FechaHora = historialmedico.FechaHora;
            existente.Estado = historialmedico.Estado; // Permitir cambiar estado

            await _repository.UpdateHistorialMedicoAsync(existente);

            return "HistorialMedico modificado correctamente";
        }

        // Caso de uso: Obtener solo historiales medicos  activos
        public async Task<IEnumerable<HistorialMedico>> ObtenerHistorialesMedicosActivosAsync()
        {
            var historialesmedicos = await _repository.GetHistorialesMedicosAsync();
            return historialesmedicos.Where(u => u.Estado == 1);
        }

        // Caso de uso: Agregar historial medico (validar duplicado)
        public async Task<string> AgregarHistorialMedicoAsync(HistorialMedico nuevoHistorialMedico)
        {
            try
            {
                var historialesmedicos = await _repository.GetHistorialesMedicosAsync();

                if (historialesmedicos.Any(u => u.Notas.ToLower() == nuevoHistorialMedico.Notas.ToLower()))
                    return "Error: ya existe un usuario con el mismo nombre";

                nuevoHistorialMedico.Estado = 1; // Activo por defecto
                var historialmedicoInsertado = await _repository.AddHistorialMedicoAsync(nuevoHistorialMedico);

                if (historialmedicoInsertado == null || historialmedicoInsertado.IdHistorialMedico <= 0)
                    return "Error: no se pudo agregar el usuario";

                return "Usuario agregado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }
    }
}
