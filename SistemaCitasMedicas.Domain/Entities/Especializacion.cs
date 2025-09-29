using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_especializacion")]
    public class Especializacion
    {
        [Key]
        [Column("idespecializacion")]
        public int IdEspecializacion { get; set; }

        [Required, MaxLength(75)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [MaxLength(100)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }
        [JsonIgnore]
        public ICollection<DoctorEspecializacion>? DoctorEspecializaciones { get; set; }

    }
}
