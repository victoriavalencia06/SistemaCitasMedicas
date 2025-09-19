using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        public long IdUsuario { get; set; }

        [Required]
        [Column("IdRol")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; }

        [Required, MaxLength(100)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(20)]
        [Column("Correo")]
        public string Correo { get; set; }

        [Required, MaxLength(255)]
        [Column("Password")]
        public string Password { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
