using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    public class Paciente
    {
        public long IdPaciente { get; set; }
        public long IdUsuario { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Seguro { get; set; }
        public int Estado { get; set; }
    }
}
