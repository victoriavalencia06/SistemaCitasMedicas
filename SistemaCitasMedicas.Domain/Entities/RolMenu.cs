using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Domain.Entities
{
    [Table("t_rol_menu")]
    public class RolMenu
    {
        [Key]
        [Column("idrolmenu")]
        public int IdRolMenu { get; set; }

        [Column("idrol")]
        public int IdRol { get; set; }

        [Column("idmenu")]
        public int IdMenu { get; set; }

        [Required]
        [Column("habilitado")]
        public bool Habilitado { get; set; }

        public Rol? Rol { get; set; }
        public Menu? Menu { get; set; }
    }
}
