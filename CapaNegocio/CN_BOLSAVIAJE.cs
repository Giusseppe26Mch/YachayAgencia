using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_BolsaViaje
    {
        private CD_BolsaViaje objCapaDato = new CD_BolsaViaje();

        /*Llamamos al método de la capadato*/

        public bool ExisteSolicitud(int idcliente, int idreserva)
        {
            return objCapaDato.ExisteSolicitud(idcliente, idreserva);
        }
        public bool Operaciones(int idcliente, int idreserva, bool sumar, out string Mensaje)
        {
            return objCapaDato.Operaciones(idcliente, idreserva, sumar, out Mensaje);
        }
        public int CantidadEnBolsa(int idcliente)
        {
            //Pasamos el parámetro
            return objCapaDato.CantidadEnBolsa(idcliente);
        }
        public List<Bolsadeviaje> ListarReserva(int idcliente)
        {
            return objCapaDato.ListarReserva(idcliente);
        }
        public bool Eliminarviaje(int idcliente, int idreserva)
        {
            return objCapaDato.Eliminarviaje(idcliente, idreserva);
        }
    }
}
