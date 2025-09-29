using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_doctor")]
    public class Doctor
    {
        [Key]
        [Column("id_doctor")]
        public int IdDoctor { get; set; }

        [Required]
        [Column("idusuario")]
        public long IdUsuario { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [Required, MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        [Column("apellido")]
        public string Apellido { get; set; }

        [Required, MaxLength(100)]
        [Column("cedulaprofesional")]
        public string CedulaProfesional { get; set; }

        [Required, StringLength(8)]
        [Column("telefono")]
        public string Telefono { get; set; }

        [Required]
        [Column("horario")]
        public DateTime Horario { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }

        [JsonIgnore]
        public ICollection<DoctorEspecializacion>? DoctorEspecializaciones { get; set; }
        [JsonIgnore]
        public ICollection<Cita>? Citas { get; set; }

    }
}
