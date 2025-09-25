using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_cita")]
    public class Cita
    {
        [Key]
        [Column("idcita")]
        public int IdCita { get; set; }

        [Required]
        [Column("idusuario")]
        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
        [Column("idpaciente")]
        public int IdPaciente { get; set; }

        public Paciente Paciente { get; set; }

        [Required]
        [Column("id_doctor")]
        public int IdDoctor { get; set; }

        public Doctor Doctor { get; set; }

        [Required]
        [Column("fechahora")]
        public DateTime FechaHora { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }
    }
}
