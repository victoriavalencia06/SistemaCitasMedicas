using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_paciente")]
    public class Paciente
    {
        [Key]
        [Column("idpaciente")]
        public int IdPaciente { get; set; }

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

        [Required]
        [Column("fechanacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(8)]
        [Column("telefono")]
        public string Telefono { get; set; }

        [Required, MaxLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; }

        [Required, MaxLength(100)]
        [Column("seguro")]
        public string Seguro { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }

        [JsonIgnore]
        public ICollection<Cita>? Citas { get; set; }

        [JsonIgnore]
        public ICollection<HistorialMedico>? Historiales { get; set; }
    }
}
