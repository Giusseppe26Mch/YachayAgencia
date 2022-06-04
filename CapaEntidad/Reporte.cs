using System;

namespace CapaEntidad
{
    public class Reporte
    {
        public String FechaVenta { get; set; }

        public String Cliente { get; set; }

        public String Reserva { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public String IdTransaccion { get; set; }

    }
}
