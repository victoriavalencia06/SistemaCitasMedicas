using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("Especializacion")]
    public class Especializacion
    {
        [Key]
        [Column("IdEspecializacion")]
        public int IdEspecializacion { get; set; }

        [Required, MaxLength(50)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [MaxLength(100)]
        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
