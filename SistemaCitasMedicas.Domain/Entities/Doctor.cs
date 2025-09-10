using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    public class Doctor
    {
        public int IdDoctor { get; set; }
        public long IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CedulaProfesional { get; set; }
        public string Telefono { get; set; }
        public DateTime Horario { get; set; } // Nota: En el UML dice "Hoario", probablemente sea "Horario"

        // Propiedad de navegación
        public Usuario Usuario { get; set; }
    }
}
