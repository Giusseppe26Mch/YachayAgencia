using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();
        public List<Categoria>Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(Categoria obj, out string Mensaje)
        {
            //Aplicamos reglas de negocio//
            Mensaje = string.Empty;
            //Indicamos que el nombre del usuario no sea vacío

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede ser vacío";
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
        public bool Editar(Categoria obj, out string Mensaje)

        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede ser vacío";
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
