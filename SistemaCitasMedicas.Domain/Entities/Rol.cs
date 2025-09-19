using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("Rol")]
    public class Rol
    {
        [Key]
        [Column("IdRol")]
        public int IdRol { get; set; }

        [Required, MaxLength(20)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
