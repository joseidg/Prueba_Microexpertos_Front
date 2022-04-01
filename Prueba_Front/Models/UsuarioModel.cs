using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Front.Models
{
    public class UsuarioModel
    {
        public int id_usuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nombre { get; set; }
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string email { get; set; }
        [Required(ErrorMessage = "Fecha es obligatoria")]
        public DateTime fecha_registro { get; set; }
        public bool activo{ get; set; }
        public DateTime fecha_actualizacion { get; set; }
    }
}
