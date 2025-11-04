using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.DTOs
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
