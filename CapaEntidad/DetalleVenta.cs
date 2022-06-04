namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public Reserva oReserva { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public string IdTransaccion { get; set; }


    }
}
