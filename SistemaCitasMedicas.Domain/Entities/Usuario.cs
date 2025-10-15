using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Rol? Rol { get; set; }

        [Required, MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        [Column("correo")]
        public string Correo { get; set; }

        [Required, MaxLength(255)]
        [Column("password")]
        public string PasswordHash { get; set; }

        // Campo transitorio: NO va a BD; solo para entrada (registro/login)
        [NotMapped]
        public string? Password { get; set; }   // vendrá en el JSON del request

        [Required]
        [Column("estado")]
        public int Estado { get; set; }

        [JsonIgnore]
        public ICollection<Cita>? Citas { get; set; }

        // Relación 1:1 opcional

        [JsonIgnore]
        public Paciente? Paciente { get; set; }
        [JsonIgnore]
        public Doctor? Doctor { get; set; }
    }
}
