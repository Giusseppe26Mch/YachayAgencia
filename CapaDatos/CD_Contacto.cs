using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Contacto
    {

        public List<Contacto> Listar()
        {
            List<Contacto> lista = new List<Contacto>();

            try
            {

                //Establecemos conexión a la conexión BD//
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "Select *from CONTACTO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                 new Contacto()
                                 {
                                     IdContacto = Convert.ToInt32(dr["IdContacto"]),
                                     Nombre = dr["Nombre"].ToString(),
                                     Apellido = dr["Apellido"].ToString(),
                                     Celular = Convert.ToInt32(dr["Celular"]),
                                     Comentario = dr["Comentario"].ToString()
                                 }

                                );


                        }


                    }
                }


            }
            catch
            {
                lista = new List<Contacto>();
            }

            return lista;
        }



        public int Registrar(Contacto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos a los procedimientos almacenados
                    SqlCommand cmd = new SqlCommand("sp_RegistrarContacto", oconexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("Celular", obj.Celular);
                    cmd.Parameters.AddWithValue("Comentario", obj.Comentario);
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
    }
}
