
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CapaDatos;
using CapaEntidad;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            // Filtrar el ID de usuario y realizar un select donde ID de Usuario sea el que se ha buscado//
            return objCapaDato.Listar();

        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            //Aplicamos reglas de negocio//
            Mensaje = string.Empty;
            //Indicamos que el nombre del usuario no sea vacío

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El Nombre del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del usuario no puede estar vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                //Procedemos a realizar una instrucción para crear una contraseña al correo del usuario//
                string clave = CN_Recursos.GenerarClave();
                string asunto = "Creación de cuenta";
                string mensaje_correo = "<h3> Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }

            }
            else
            {
                return 0;
            }


        }

        public bool Editar(Usuario obj, out string Mensaje)

        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El Nombre del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del usuario no puede estar vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje); /*Encriptamos la nueva clave*/

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3> Su cuenta ha sido reestablecida correctamente.</h3><br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se puso reestablecer la contraseña";
                return false;
            }



        }
    }
}
