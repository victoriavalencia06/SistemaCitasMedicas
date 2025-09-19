using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("Doctor")]
    public class Doctor
    {
        [Key]
        [Column("IdDoctor")]
        public int IdDoctor { get; set; }

        [Required]
        [Column("IdUsuario")]
        public long IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        [Required, MaxLength(50)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(50)]
        [Column("Apellido")]
        public string Apellido { get; set; }

        [Required, MaxLength(50)]
        [Column("CedulaProfesional")]
        public string CedulaProfesional { get; set; }

        [Required, StringLength(8)]
        [Column("Telefono")]
        public string Telefono { get; set; }

        [Required]
        [Column("Horario")]
        public DateTime Horario { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
