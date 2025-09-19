using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("HistorialMedico")]
    public class HistorialMedico
    {
        [Key]
        [Column("IdHistorialMedico")]
        public int IdHistorialMedico { get; set; }

        [Required]
        [Column("IdPaciente")]
        public int IdPaciente { get; set; }

        [ForeignKey("IdPaciente")]
        public Paciente Paciente { get; set; }

        [Column("Notas")]
        public string Notas { get; set; }

        [Required]
        [Column("Diagnostico")]
        public string Diagnostico { get; set; }

        [Column("Tratamientos")]
        public string Tratamientos { get; set; }

        [Column("CuadroMedico")]
        public string CuadroMedico { get; set; }

        [Required]
        [Column("FechaHora")]
        public DateTime FechaHora { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
