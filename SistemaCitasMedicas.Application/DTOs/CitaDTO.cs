using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class CitaDTO
    {
        public int IdCita { get; set; }
        public string Paciente { get; set; }
        public string Doctor { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Estado { get; set; } // true = activa, false = cancelada
    }

}
