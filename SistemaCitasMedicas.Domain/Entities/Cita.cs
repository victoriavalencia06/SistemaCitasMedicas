using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    public class Cita
    {
        public int IdCita { get; set; }
        public long IdUsuario { get; set; }
        public long IdPaciente { get; set; }
        public int IdDoctor { get; set; }
        public DateTime FechaHora { get; set; }
        public int Estado { get; set; }
    }
}
