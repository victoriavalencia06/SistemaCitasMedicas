using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_doctor_especializacion")]
    public class DoctorEspecializacion
    {
        [Key]
        [Column("idoctorespecializacion")]
        public int IdDoctorEspecialidad { get; set; }

        [Required]
        [Column("id_doctor")]
        public int IdDoctor { get; set; }
        [JsonIgnore]
        public Doctor Doctor { get; set; }

        [Required]
        [Column("idespecializacion")]
        public int IdEspecializacion { get; set; }

        [JsonIgnore]
        public Especializacion Especializacion { get; set; }
    }
}
