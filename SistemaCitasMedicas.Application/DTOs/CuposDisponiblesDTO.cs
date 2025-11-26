using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class CuposDisponiblesDTO
    {
        public string Fecha { get; set; } = "";       // "YYYY-MM-DD"
        public int TotalCupos { get; set; }           // ej. 20
        public int CitasConfirmadas { get; set; }     // ej. 12
        public int CuposDisponibles { get; set; }     // ej. 8
    }
}
