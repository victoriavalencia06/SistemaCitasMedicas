using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    public class DoctorEspecializacion
    {
        public int IdDoctorEspecializacion { get; set; }
        public int IdDoctor { get; set; }
        public int IdEspecializacion { get; set; }

        // Propiedades de navegación
        public Doctor Doctor { get; set; }
        public Especializacion Especializacion { get; set; }
    }
}
