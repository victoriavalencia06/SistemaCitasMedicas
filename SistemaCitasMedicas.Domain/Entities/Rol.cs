using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_rol")]
    public class Rol
    {
        [Key]
        [Column("idrol")]
        public int IdRol { get; set; }

        [Required, MaxLength(75)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [Column("estado")]
        public int Estado { get; set; }

        [JsonIgnore]
        public ICollection<Usuario>? Usuarios { get; set; }
        [JsonIgnore]
        public ICollection<RolMenu>? RolMenus { get; set; }

    }
}
