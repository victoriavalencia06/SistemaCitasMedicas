using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("Paciente")]
    public class Paciente
    {
        [Key]
        [Column("IdPaciente")]
        public int IdPaciente { get; set; }

        [Required]
        [Column("IdUsuario")]
        public long IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        [Required, MaxLength(100)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        [Column("Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Column("FechaNacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(8)]
        [Column("Telefono")]
        public string Telefono { get; set; }

        [Required, MaxLength(150)]
        [Column("Direccion")]
        public string Direccion { get; set; }

        [Required, MaxLength(100)]
        [Column("Seguro")]
        public string Seguro { get; set; }

        [Required]
        [Column("Estado")]
        public int Estado { get; set; }
    }
}
