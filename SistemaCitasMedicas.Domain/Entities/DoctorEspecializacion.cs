using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("DoctorEspecializacion")]
    public class DoctorEspecializacion
    {
        [Key]
        [Column("IdDoctorEspecialidad")]
        public int IdDoctorEspecialidad { get; set; }

        [Required]
        [Column("IdDoctor")]
        public int IdDoctor { get; set; }

        [ForeignKey("IdDoctor")]
        public Doctor Doctor { get; set; }

        [Required]
        [Column("IdEspecializacion")]
        public int IdEspecializacion { get; set; }

        [ForeignKey("IdEspecializacion")]
        public Especializacion Especializacion { get; set; }
    }
}
