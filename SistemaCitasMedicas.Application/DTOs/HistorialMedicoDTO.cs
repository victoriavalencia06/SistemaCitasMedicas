using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class HistorialMedicoDTO
    {
        [Required]
        public int IdPaciente { get; set; }

        [Required]
        public int IdCita { get; set; }

        public string Notas { get; set; }

        [Required]
        public string Diagnostico { get; set; }

        public string Tratamientos { get; set; }

        public string CuadroMedico { get; set; }

        public string Alergias { get; set; }

        public string AntecedentesFamiliares { get; set; }

        public string Observaciones { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public int Estado { get; set; }
    }
}
