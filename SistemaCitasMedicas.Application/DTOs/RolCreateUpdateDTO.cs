using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class RolCreateUpdateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public int Estado { get; set; } = 1; // mantén int para coincidir con tu modelo
        public List<RolMenuDTO> Permisos { get; set; } = new();
    }
}
