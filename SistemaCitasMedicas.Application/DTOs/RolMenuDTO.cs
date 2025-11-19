using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class RolMenuDTO
    {
        public int MenuId { get; set; }
        public string NombreMenu { get; set; } = string.Empty;
        public bool Habilitado { get; set; }
    }
}
