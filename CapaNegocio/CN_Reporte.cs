using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapaDato = new CD_Reporte();

        public List<Reporte>Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            return objCapaDato.Ventas(fechainicio, fechafin, idtransaccion);
        }

        //Agregar parámetros de objeto a la clase Tablero
        public Tablero VerTablero()
        {
            return objCapaDato.VerTablero();
        }
    }
}
