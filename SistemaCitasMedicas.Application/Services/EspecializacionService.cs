using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public class EspecializacionService
    {
        private readonly IEspecializacionRepository _especializacionRepository;

        public EspecializacionService(IEspecializacionRepository especializacionRepository)
        {
            _especializacionRepository = especializacionRepository;
        }

        //obtener todas las especializaciones
        public async Task<IEnumerable<Especializacion>> ObtenerTodasLasEspecializaciones()
        {
            var especializaciones = await _especializacionRepository.GetEspecializacionesAsync();
            return especializaciones.Where(e => e.Estado == 1);
        }

        //obtener una especializacion por id
        public async Task<Especializacion> ObtenerEspecializacionPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la especialización debe ser mayor que cero.");

            var especializacion = await _especializacionRepository.GetEspecializacionByIdAsync(id);
            if (especializacion != null && especializacion.Estado == 1)
            {
                return especializacion;
            }

            throw new KeyNotFoundException("Especialización no encontrada o no disponible.");
        }

        //agregar una nueva especializacion
        public async Task<string> AgregarEspecializacion(Especializacion especializacion)
        {
            try
            {
                var especializaciones = await _especializacionRepository.GetEspecializacionesAsync();
                if (especializaciones.Any(e => e.Nombre.ToLower() == especializacion.Nombre.ToLower()))
                {
                    return "Error: ya existe una especialización con ese nombre";
                }

                especializacion.Estado = 1; // Por defecto la especialización se agrega como activa
                var especializacionInsertada = await _especializacionRepository.AddEspecializacionAsync(especializacion);
                if (especializacionInsertada == null || especializacionInsertada.IdEspecializacion <= 0)
                {
                    return "Error: no se pudo insertar la especialización";
                }
                return "Especialización agregada exitosamente.";
            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }

        //modificar una especializacion existente
        public async Task<string> ModificarEspecializacion(Especializacion especializacion)
        {
            try
            {
                if (especializacion == null || especializacion.IdEspecializacion <= 0)
                {
                    return "Error: especialización inválida.";
                }
                var especializacionExistente = await _especializacionRepository.GetEspecializacionByIdAsync(especializacion.IdEspecializacion);

                if (especializacionExistente == null)
                {
                    return "Error: la especialización no existe.";
                }
                especializacionExistente.Nombre = especializacion.Nombre;
                especializacionExistente.Descripcion = especializacion.Descripcion;
                especializacionExistente.Estado = especializacion.Estado;

                await _especializacionRepository.UpdateEspecializacionAsync(especializacionExistente);  
                return "Especialización modificada exitosamente.";

            }
            catch (Exception ex)
            {
                return $"Error de servidor: {ex.Message}";
            }
        }


    }
}
