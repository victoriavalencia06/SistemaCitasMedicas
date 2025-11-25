using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Application.DTOs;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/historialmedico")]
    [Authorize]
    public class HistorialMedicoController : ControllerBase
    {
        private readonly HistorialMedicoServices _historialMedicoService;

        public HistorialMedicoController(HistorialMedicoServices historialMedicoService)
        {
            _historialMedicoService = historialMedicoService;
        }

        // GET: api/historialmedico/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<HistorialMedico>>> Get()
        {
            var historialesmedicos = await _historialMedicoService.ObtenerHistorialesMedicosActivosAsync();
            return Ok(historialesmedicos);
        }

        // GET: api/historialmedico/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<HistorialMedico>> GetById(int id)
        {
            try
            {
                var historialmedico = await _historialMedicoService.ObtenerHistorialMedicoPorIdAsync(id);

                if (historialmedico == null)
                    return NotFound($"No se encontró un historial médico con ID {id}");

                return Ok(historialmedico);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/historialmedico/paciente/5
        [HttpGet("paciente/{idPaciente}")]
        public async Task<ActionResult<IEnumerable<HistorialMedico>>> GetByPaciente(int idPaciente)
        {
            try
            {
                var historiales = await _historialMedicoService.ObtenerHistorialesPorPacienteAsync(idPaciente);
                return Ok(historiales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/historialmedico/cita/5
        [HttpGet("cita/{idCita}")]
        public async Task<ActionResult<HistorialMedico>> GetByCita(int idCita)
        {
            try
            {
                var historial = await _historialMedicoService.ObtenerHistorialPorCitaAsync(idCita);

                if (historial == null)
                    return NotFound($"No se encontró un historial médico para la cita con ID {idCita}");

                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/historialmedico/create
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] HistorialMedicoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Crear la entidad HistorialMedico desde el DTO
                var historialmedico = new HistorialMedico
                {
                    IdPaciente = dto.IdPaciente,
                    IdCita = dto.IdCita,
                    Notas = dto.Notas,
                    Diagnostico = dto.Diagnostico,
                    Tratamientos = dto.Tratamientos,
                    CuadroMedico = dto.CuadroMedico,
                    Alergias = dto.Alergias,
                    AntecedentesFamiliares = dto.AntecedentesFamiliares,
                    Observaciones = dto.Observaciones,
                    FechaHora = dto.FechaHora,
                    Estado = dto.Estado,
                    // Las propiedades de navegación se mantienen null por defecto
                    Paciente = null,
                    Cita = null
                };

                var resultado = await _historialMedicoService.AgregarHistorialMedicoAsync(historialmedico);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(new { message = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/historialmedico/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] HistorialMedicoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Crear la entidad HistorialMedico desde el DTO
                var historialmedico = new HistorialMedico
                {
                    IdHistorialMedico = id, // ID desde la ruta
                    IdPaciente = dto.IdPaciente,
                    IdCita = dto.IdCita,
                    Notas = dto.Notas,
                    Diagnostico = dto.Diagnostico,
                    Tratamientos = dto.Tratamientos,
                    CuadroMedico = dto.CuadroMedico,
                    Alergias = dto.Alergias,
                    AntecedentesFamiliares = dto.AntecedentesFamiliares,
                    Observaciones = dto.Observaciones,
                    FechaHora = dto.FechaHora,
                    Estado = dto.Estado,
                    Paciente = null,
                    Cita = null
                };

                var resultado = await _historialMedicoService.ModificarHistorialMedicoAsync(historialmedico);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(new { message = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/historialmedico/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _historialMedicoService.DesactivarHistorialMedicoAsync(id);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(new { message = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}