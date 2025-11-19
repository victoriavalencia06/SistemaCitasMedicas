using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_menu")]
    public class Menu
    {
        [Key]
        [Column("idmenu")]
        public int IdMenu { get; set; }

        [Required, MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(150)]
        [Column("ruta")]
        public string? Ruta { get; set; }

        [MaxLength(100)]
        [Column("icono")]
        public string? Icono { get; set; }

        [Column("orden")]
        public int Orden { get; set; }
        [JsonIgnore]
        public ICollection<RolMenu>? RolMenus { get; set; }
    }
}
