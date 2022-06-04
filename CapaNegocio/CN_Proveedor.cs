using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Proveedores
    {
        private CD_Proveedores objCapaDato = new CD_Proveedores();

        public List<Proveedor> Listar()
        {
            // Filtrar el ID de Proveedor y realizar un select donde ID de Proveedor sea el que se ha buscado//
            return objCapaDato.Listar();

        }

        public int Registrar(Proveedor obj, out string Mensaje)
        {
            //Aplicamos reglas de negocio//
            Mensaje = string.Empty;
            //Indicamos que el nombre del Proveedor no sea vacío

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del Proveedor no puede estar vacío";
            }
            else if (obj.RUC == 0) /*Valores Enteros*/
            {
                Mensaje = "El campo de RUC no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.RepresentanteLegal) || string.IsNullOrWhiteSpace(obj.RepresentanteLegal))
            {
                Mensaje = "El campo de Representante no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Direccion) || string.IsNullOrWhiteSpace(obj.Direccion))
            {
                Mensaje = "El campo de dirección no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Ciudad) || string.IsNullOrWhiteSpace(obj.Ciudad))
            {
                Mensaje = "El campo de ciudad no puede estar vacío";
            }
            else if (obj.Telefono == 0)
            {
                Mensaje = "El campo de teléfono no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correocontacto) || string.IsNullOrWhiteSpace(obj.Correocontacto))
            {
                Mensaje = "El campo de correo no puede estar vacío";

            }
            else if (string.IsNullOrEmpty(obj.TipoServicio) || string.IsNullOrWhiteSpace(obj.TipoServicio))
            {
                Mensaje = "El Tipo de servicio no puede estar vacío";

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

        public bool Editar(Proveedor obj, out string Mensaje)

        {
            Mensaje = string.Empty;
            //Indicamos que el nombre del Proveedor no sea vacío

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del Proveedor no puede estar vacío";
            }
            else if (obj.RUC == 0) /*Valores Enteros*/
            {
                Mensaje = "El campo de RUC no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.RepresentanteLegal) || string.IsNullOrWhiteSpace(obj.RepresentanteLegal))
            {
                Mensaje = "El campo de Representante no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Direccion) || string.IsNullOrWhiteSpace(obj.Direccion))
            {
                Mensaje = "El campo de dirección no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Ciudad) || string.IsNullOrWhiteSpace(obj.Ciudad))
            {
                Mensaje = "El campo de ciudad no puede estar vacío";
            }
            else if (obj.Telefono == 0)
            {
                Mensaje = "El campo de teléfono no puede estar vacío";
            }

            else if (string.IsNullOrEmpty(obj.TipoServicio) || string.IsNullOrWhiteSpace(obj.TipoServicio))
            {
                Mensaje = "El Tipo de servicio no puede estar vacío";

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
    }
}
