﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaEntidad
{
    public class Bolsadeviaje

    {
        public int IdBolsadeviaje { get; set; }

        public Cliente oCliente { get; set; }

        public Reserva oReserva { get; set; }

        public int Cantidad { get; set; }


    }
}
