
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class Cliente
    {
      
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "Este campo es Requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede contener más de 100 caracteres. ")]
        [MinLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres. ")]
        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Este campo es Requerido.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Formato incorrecto.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{8,16}$", ErrorMessage = "La contraseña debe tener entre 8 y 16 caracteres, al menos un dígito, al menos " +
            "una " +"minúscula y" + " al menos una mayúscula.")]
        public string Clave { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClave { get; set; }

        public bool Reestablecer { get; set; }
    }
}
