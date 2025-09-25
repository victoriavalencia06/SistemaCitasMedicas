using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_paciente")]
    public class Paciente
    {
        [Key]
        [Column("idpaciente")]
        public int IdPaciente { get; set; }

        [Required]
        [Column("idusuario")]
        public long IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [Required, MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        [Column("apellido")]
        public string Apellido { get; set; }

        [Required]
        [Column("fechanacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(8)]
        [Column("telefono")]
        public string Telefono { get; set; }

        [Required, MaxLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; }

        [Required, MaxLength(100)]
        [Column("seguro")]
        public string Seguro { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }

        public ICollection<Cita>? Citas { get; set; }
        public HistorialMedico? HistorialMedico { get; set; }

    }
}
