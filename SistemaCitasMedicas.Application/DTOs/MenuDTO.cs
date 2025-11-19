using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class MenuDTO
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ruta { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
    }
}
