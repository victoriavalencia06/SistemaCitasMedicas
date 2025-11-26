using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    // Algoritmos con lógica de negocio (UseCase)
    public class CitaService
    {
        private readonly ICitaRepository _repository;
        public CitaService(ICitaRepository repository)
        {
            _repository = repository;
        }

        // 1. Caso de uso: Obtener todas las citas (activas e inactivas)
        public async Task<IEnumerable<Cita>> ObtenerTodasCitasAsync()
        {
            return await _repository.GetCitasAsync();
        }

        // 2. Caso de uso: Buscar una cita por Id (solo activas)
        public async Task<Cita?> ObtenerCitaPorIdAsync(int idCita)
        {
            if (idCita <= 0)
                return null; // Id inválido

            var cita = await _repository.GetCitaByIdAsync(idCita);

            if (cita != null && cita.Estado == 1) // 1 = Activa
                return cita;

            return null; // No encontrada o inactiva
        }

        // 3. Caso de uso: Agregar una nueva cita (validar duplicados)
        public async Task<string> AgregarCitaAsync(Cita nuevaCita)
        {
            try
            {
                // Validación: evitar citas duplicadas
                if (await _repository.ExisteCitaDuplicadaAsync(nuevaCita))
                    return "Error: El paciente ya tiene una cita registrada en esa fecha y hora";

                nuevaCita.Estado = 1; // Activa por defecto

                var citaInsertada = await _repository.AddCitaAsync(nuevaCita);

                if (citaInsertada == null || citaInsertada.IdCita <= 0)
                    return "Error: No se pudo agregar la cita";

                return "Cita agregada correctamente";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }

        // 4. Caso de uso: Modificar una cita existente
        public async Task<string> ModificarCitaAsync(Cita cita)
        {
            var existente = await _repository.GetCitaByIdAsync(cita.IdCita);

            if (existente == null)
                return "Error: Cita no encontrada";

            if (await _repository.ExisteCitaDuplicadaAsync(cita))
                return "Error: Ya existe una cita para el paciente en esa fecha y hora";

            existente.IdPaciente = cita.IdPaciente;
            existente.IdDoctor = cita.IdDoctor;
            existente.FechaHora = cita.FechaHora;
            existente.Estado = cita.Estado;

            await _repository.UpdateCitaAsync(existente);

            return "Cita modificada correctamente";
        }


        // Cambia estado a inactivo en lugar de eliminar
        public async Task<string> DesactivarCitaPorIdAsync(int id) 
        {
            var doctor = await _repository.GetCitaByIdAsync(id);
            if (doctor == null || doctor.Estado == 0) return "Error: la cita no existe o ya está inactivo.";

            doctor.Estado = 0;
            await _repository.UpdateCitaAsync(doctor);
            return "Cita desactivado exitosamente.";
        }

        // Obtener citas de un mes (rango)
        public async Task<IEnumerable<Cita>> ObtenerCitasPorMesAsync(int year, int month)
        {
            if (month < 1 || month > 12) return Enumerable.Empty<Cita>();
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            return await _repository.GetCitasByRangeAsync(start, end);
        }

        // Obtener citas de un día
        public async Task<IEnumerable<Cita>> ObtenerCitasPorDiaAsync(DateTime date)
        {
            return await _repository.GetCitasByDayAsync(date);
        }

        // Obtener conteos por día para un mes
        public async Task<Dictionary<DateTime, int>> ObtenerConteosPorMesAsync(int year, int month)
        {
            return await _repository.GetCountsByMonthAsync(year, month);
        }
    }
}
