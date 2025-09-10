using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    public class HistorialMedico
    {
        public int IdHistorialMedico { get; set; }
        public int IdPaciente { get; set; }
        public string Notas { get; set; }
        public string Descripcion { get; set; }
        public string Tratamiento { get; set; }
        public string CuadroMedico { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
