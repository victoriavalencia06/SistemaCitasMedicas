using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("Cita")]
    public class Cita
    {
        [Key]
        [Column("IdCita")]
        public int IdCita { get; set; }

        [Required]
        [Column("IdUsuario")]
        public long IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        [Required]
        [Column("IdPaciente")]
        public int IdPaciente { get; set; }

        [ForeignKey("IdPaciente")]
        public Paciente Paciente { get; set; }

        [Required]
        [Column("IdDoctor")]
        public int IdDoctor { get; set; }

        [ForeignKey("IdDoctor")]
        public Doctor Doctor { get; set; }

        [Required]
        [Column("FechaHora")]
        public DateTime FechaHora { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
