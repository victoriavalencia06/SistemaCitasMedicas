using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services
{
    public class DoctorEspecializacionService
    {
        private readonly IDoctorEspecializacionRepository _repo;

        public DoctorEspecializacionService(IDoctorEspecializacionRepository repo) =>
            _repo = repo;

        public async Task<IEnumerable<DoctorEspecializacion>> ObtenerEspecializacionesDeDoctor(int doctorId) =>
            await _repo.GetEspecializacionesPorDoctorAsync(doctorId);

        public async Task<IEnumerable<DoctorEspecializacion>> ObtenerDoctoresPorEspecializacion(int especializacionId) =>
            await _repo.GetDoctoresPorEspecializacionAsync(especializacionId);

        public async Task<string> AsignarEspecializacion(int doctorId, int especializacionId)
        {
            var relacion = new DoctorEspecializacion
            {
                IdDoctor = doctorId,
                IdEspecializacion = especializacionId
            };

            await _repo.AddDoctorEspecializacionAsync(relacion);
            return "Especialización asignada al doctor.";
        }

        public async Task<string> QuitarEspecializacion(int doctorId, int especializacionId)
        {
            var eliminado = await _repo.DeleteDoctorEspecializacionAsync(doctorId, especializacionId);
            return eliminado ? "Especialización removida del doctor." : "Error: relación no encontrada.";
        }
    }
}
