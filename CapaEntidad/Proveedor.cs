

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaEntidad
{
    public class Proveedor

    {
        public int IdProveedor { get; set; }

        public String Nombre { get; set; }

        public int RUC { get; set; }

        public String RepresentanteLegal { get; set; }

        public String Direccion { get; set; }

        public String Ciudad { get; set; }

        public int Telefono { get; set; }

        public String Correocontacto { get; set; }

        public String TipoServicio { get; set; }


        public bool Activo { get; set; }

    }
}
