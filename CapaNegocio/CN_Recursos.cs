﻿using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace CapaNegocio
{
    public class CN_Recursos
    {

        //ENCRIPTACION, CONVERSION FORMATO IMAGENES
        //MÉTODOS QUE PERMITAN GENERAR CLAVES AUTOMÁTICAS

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);    //Permite retornar un código único C#
            return clave;

        }

        public static string Generarnuevonombre()
        {
            string nombre = Guid.NewGuid().ToString("N").Substring(0, 6);    //Permite retornar un código único C#
            return nombre;

        }



        //Encriptación de TEXTO en SHA256

        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            //Usar la referencia de "System.Security.Cryptography en la línea 19"

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


        //MÉTODO QUE ME PERMITE ENVIAR UN CORREO

        public static bool EnviarCorreo(string correo, string asunto, string mensaje) // Recibe 3 parámetros
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("giusseppemch@gmail.com"); // Correo que se encargará de enviar a otros usuarios
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("giusseppemch@gmail.com", "wqyqmdtpuktoeovk"), // Correo y contraseña específica
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(mail);
                resultado = true;


            }

            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }
        //Crear método de convertir texto a un formato base 64//

        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true; //Parámetro de salida
            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false;

            }
            return textoBase64;
        }


    }
}
