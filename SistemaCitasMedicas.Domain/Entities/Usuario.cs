using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_usuario")]
    public class Usuario
    {
        [Key]
        [Column("idusuario")]
        public long IdUsuario { get; set; }

        [Required]
        [Column("idrol")]
        public int IdRol { get; set; }
        public Rol Rol { get; set; }

        [Required, MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        [Column("correo")]
        public string Correo { get; set; }

        [Required, MaxLength(255)]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }

        public ICollection<Cita>? Citas { get; set; }

        // Relación 1:1 opcional
        public Paciente? Paciente { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
