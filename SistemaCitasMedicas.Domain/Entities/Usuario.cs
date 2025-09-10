using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    public class Usuario
    {
        public long IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public int Estado { get; set; }

        // Propiedad de navegación para la relación con Rol
        public Rol Rol { get; set; }
    }
}
