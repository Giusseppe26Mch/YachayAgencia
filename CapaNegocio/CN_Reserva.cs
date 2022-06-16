using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Reserva
    {
        private CD_Reserva objCapaDato = new CD_Reserva();
        public List<Reserva>Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(Reserva obj, out string Mensaje)
        {
            //Aplicamos reglas de negocio//
            Mensaje = string.Empty;
            //Indicamos que el nombre del usuario no sea vacío

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la Reserva no puede estar vacío";
            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la Reserva no puede estar vacío";
            }

            else if (obj.oPaquete.IdPaquete == 0)
            {
                Mensaje = "Debe seleccionar un paquete";
            }

            else if (obj.ocategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria";
            }

            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio de la reserva";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock de la reserva";
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
        public bool Editar(Reserva obj, out string Mensaje)

        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la Reserva no puede estar vacío";
            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la Reserva no puede estar vacío";
            }

            else if (obj.oPaquete.IdPaquete == 0)
            {
                Mensaje = "Debe seleccionar un paquete";
            }

            else if (obj.ocategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria";
            }

            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio de la reserva";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock de la reserva";
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
        public bool GuardarDatosImagen(Reserva obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }


        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }


    }
}
