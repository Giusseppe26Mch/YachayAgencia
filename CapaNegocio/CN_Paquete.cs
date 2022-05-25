using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Paquete
    {
        private CD_Paquete objCapaDato = new CD_Paquete();
        public List<PaqueteTuristico> Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(PaqueteTuristico obj, out string Mensaje)
        {
            //Aplicamos reglas de negocio//
            Mensaje = string.Empty;
            //Indicamos que el nombre del usuario no sea vacío

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del paquete no puede ser vacío";
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
        public bool Editar(PaqueteTuristico obj, out string Mensaje)

        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del paquete no puede ser vacío";
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
        public List<PaqueteTuristico> ListarPaqueteporCategoria(int idcategoria)
        {
            return objCapaDato.ListarPaqueteporCategoria(idcategoria);
        }
    }
}
