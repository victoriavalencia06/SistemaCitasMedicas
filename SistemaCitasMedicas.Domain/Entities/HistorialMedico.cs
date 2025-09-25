using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_historial_medico")]
    public class HistorialMedico
    {
        [Key]
        [Column("idhistorialmedico")]
        public int IdHistorialMedico { get; set; }

        [Required]
        [Column("idpaciente")]
        public int IdPaciente { get; set; }
        public Paciente Paciente { get; set; }

        [Column("notas")]
        public string Notas { get; set; }

        [Required]
        [Column("diagnostico")]
        public string Diagnostico { get; set; }

        [Column("tratamientos")]
        public string Tratamientos { get; set; }

        [Column("cuadro_medico")]
        public string CuadroMedico { get; set; }

        [Required]
        [Column("fechahora")]
        public DateTime FechaHora { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }
    }
}
