using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ReporteTablero", oconexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                 new Reporte()
                                 {
                                     FechaVenta = dr["FechaVenta"].ToString(),
                                     Cliente = dr["Cliente"].ToString(),
                                     Reserva = dr["Reserva"].ToString(),
                                     Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                     Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                     Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                     IdTransaccion = dr["IdTransaccion"].ToString(),
                                 }
                                );



                        }


                    }
                }


            }
            catch
            {
                lista = new List<Reporte>();
            }

            return lista;
        }





        public Tablero VerTablero()
        {
            Tablero objeto = new Tablero();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ReporteTablero", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure; //Indicamos que es un procedimiento almacenado//
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new Tablero()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalReserva = Convert.ToInt32(dr["TotalReserva"]),

                            };
                        }


                    }
                }


            }
            catch
            {
                objeto = new Tablero();
            }

            return objeto;
        }












    }
}
