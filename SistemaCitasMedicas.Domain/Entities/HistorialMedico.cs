using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Paciente? Paciente { get; set; }

        [Required]
        [Column("idcita")]
        public int IdCita { get; set; }
        [JsonIgnore]
        public Cita? Cita { get; set; }

        [Column("notas")]
        public string Notas { get; set; }

        [Required]
        [Column("diagnostico")]
        public string Diagnostico { get; set; }

        [Column("tratamientos")]
        public string Tratamientos { get; set; }

        [Column("cuadroMedico")]
        public string CuadroMedico { get; set; }

        [Column("alergias")]
        public string? Alergias { get; set; }

        [Column("antecedentesFamiliares")]
        public string? AntecedentesFamiliares { get; set; }

        [Column("observaciones")]
        public string? Observaciones { get; set; }

        [Required]
        [Column("fechahora")]
        public DateTime FechaHora { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }
    }
}