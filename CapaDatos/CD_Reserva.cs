using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace CapaDatos
{
    public class CD_Reserva
    {
        public List<Reserva> Listar()
        {
            List<Reserva> lista = new List<Reserva>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select r.IdReserva, r.Nombre,r.Descripcion,");
                    sb.AppendLine("p.IdPaquete,p.Descripcion[DesPaquete],");
                    sb.AppendLine("c.IdCategoria,c.Descripcion[DesCategoria],");
                    sb.AppendLine("r.Precio,r.Stock,r.RutaImagen,r.NombreImagen,r.Activo");
                    sb.AppendLine("from Reserva r");
                    sb.AppendLine("inner join PaqueteTuristico p on p.IdPaquete = r.IdPaquete");
                    sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = r.IdCategoria");



                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                 new Reserva()
                                 {
                                     IdReserva = Convert.ToInt32(dr["IdReserva"]),
                                     Nombre = dr["Nombre"].ToString(),
                                     Descripcion = dr["Descripcion"].ToString(),
                                     oPaquete = new PaqueteTuristico() { IdPaquete = Convert.ToInt32(dr["IdPaquete"]), Descripcion = dr["DesPaquete"].ToString() },
                                     ocategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCategoria"]), Descripcion = dr["DesCategoria"].ToString() },
                                     Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                     Stock = Convert.ToInt32(dr["Stock"]),
                                     RutaImagen = dr["RutaImagen"].ToString(),
                                     NombreImagen = dr["NombreImagen"].ToString(),
                                     Activo = Convert.ToBoolean(dr["Activo"])
                                 });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Reserva>();
            }

            return lista;
        }

        public int Registrar(Reserva obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_RegistrarReserva", oconexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdPaquete", obj.oPaquete.IdPaquete);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.ocategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output; //Parámetros de salida
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();
                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }

        public bool Editar(Reserva obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_EditarReserva", oconexion);
                    cmd.Parameters.AddWithValue("IdReserva", obj.IdReserva);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdPaquete", obj.oPaquete.IdPaquete);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.ocategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output; //Parámetros de salida
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
        public bool GuardarDatosImagen(Reserva obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))

                {
                    string query = "update Reserva set RutaImagen =@rutaimagen, NombreImagen = @nombreimagen where IdReserva = idreserva";

                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("idreserva", obj.IdReserva);

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";
                    }

                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_EliminarReserva", oconexion);
                    cmd.Parameters.AddWithValue("IdReserva", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output; //Parámetros de salida
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}


