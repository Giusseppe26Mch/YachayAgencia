using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Contacto
    {
        private CD_Contacto objCapaDato = new CD_Contacto();

        public List<Contacto> Listar()
        {
            // Filtrar el ID de Contacto y realizar un select donde ID de Contacto sea el que se ha buscado//
            return objCapaDato.Listar();
            //Alterar//

        }

        public int Registrar(Contacto obj, out string Mensaje)
        {
            //Aplicamos reglas de negocio//
            Mensaje = string.Empty;
            //Indicamos que el nombre del Proveedor no sea vacío

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El campo de Nombre no puede estar vacío";

            }
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El campo de Apellido no puede estar vacío";
            }
            else if (obj.Celular == 0) /*Valores Enteros*/
            {
                Mensaje = "El campo de Celular no puede estar vacío";
            }

            else if (string.IsNullOrEmpty(obj.Comentario) || string.IsNullOrWhiteSpace(obj.Comentario))
            {
                Mensaje = "Rellene el campo de comentario, ya que no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }

            else
            {
                return 0;
            }


        }
    }
}
