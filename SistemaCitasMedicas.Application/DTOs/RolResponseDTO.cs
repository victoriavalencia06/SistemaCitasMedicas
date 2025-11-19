using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class RolResponseDTO
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Estado { get; set; }
        public List<RolMenuDTO> Permisos { get; set; } = new();
    }
}
