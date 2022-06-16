﻿using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
namespace CapaDatos
{
    public class CD_Paquete
    {
        public List<PaqueteTuristico> Listar()
        {
            List<PaqueteTuristico> lista = new List<PaqueteTuristico>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "Select IdPaquete, Descripcion, Activo FROM  PaqueteTuristico";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                 new PaqueteTuristico()
                                 {
                                     IdPaquete = Convert.ToInt32(dr["IdPaquete"]),
                                     Descripcion = dr["Descripcion"].ToString(),
                                     Activo = Convert.ToBoolean(dr["Activo"])
                                 });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PaqueteTuristico>();
            }

            return lista;
        }

        public int Registrar(PaqueteTuristico obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPaquete", oconexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Editar(PaqueteTuristico obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_EditarPaquete", oconexion);
                    cmd.Parameters.AddWithValue("IdPaquete", obj.IdPaquete);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_EliminarPaquete", oconexion);
                    cmd.Parameters.AddWithValue("IdPaquete", id);
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
        public List<PaqueteTuristico> ListarPaqueteporCategoria(int idcategoria)
        {
            List<PaqueteTuristico> lista = new List<PaqueteTuristico>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select distinct p.IdPaquete, p.Descripcion from RESERVA r");
                    sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = r.IdCategoria");
                    sb.AppendLine("inner join PaqueteTuristico p on p.IdPaquete = r.IdPaquete and p.Activo = 1");
                    sb.AppendLine("where c.IdCategoria = iif(@idcategoria = 0, c.IdCategoria, @idcategoria)");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                 new PaqueteTuristico()
                                 {
                                     IdPaquete = Convert.ToInt32(dr["IdPaquete"]),
                                     Descripcion = dr["Descripcion"].ToString(),
                                 });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PaqueteTuristico>();
            }

            return lista;
        }
    }
}
