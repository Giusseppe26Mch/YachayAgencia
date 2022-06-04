using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentaciónAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult PaqueteTuristico()
        {
            return View();
        }
        public ActionResult Reserva()
        {
            return View();
        }

        //----------------- CATEGORÍA -----------------------//
        #region CATEGORIA
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado; //String o cualquier valor
            string mensaje = string.Empty;
            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //---------------------PAQUETE TURÍSTICO --------------------//
        #region PAQUETE
        [HttpGet]
        public JsonResult ListarPaquete()
        {
            List<PaqueteTuristico> oLista = new List<PaqueteTuristico>();
            oLista = new CN_Paquete().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarPaquete(PaqueteTuristico objeto)
        {
            object resultado; //String o cualquier valor
            string mensaje = string.Empty;
            if (objeto.IdPaquete == 0)
            {
                resultado = new CN_Paquete().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Paquete().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult EliminarPaquete(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Paquete().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //---------------------- RESERVA -------------//
        #region RESERVA
        [HttpGet]
        public JsonResult ListarReserva()
        {
            List<Reserva> oLista = new List<Reserva>();
            oLista = new CN_Reserva().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarReserva(string objeto, HttpPostedFileBase archivoImagen) // Registro
        {

            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Reserva oReserva = new Reserva();
            oReserva = JsonConvert.DeserializeObject<Reserva>(objeto);

            decimal precio;

            if (decimal.TryParse(oReserva.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oReserva.Precio = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }


            if (oReserva.IdReserva == 0)
            {
                int idReservaGenerada = new CN_Reserva().Registrar(oReserva, out mensaje);
                if (idReservaGenerada != 0)
                {
                    oReserva.IdReserva = idReservaGenerada;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }

            else
            {
                operacion_exitosa = new CN_Reserva().Editar(oReserva, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oReserva.IdReserva.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }
                    if (guardar_imagen_exito)
                    {
                        oReserva.RutaImagen = ruta_guardar;
                        oReserva.NombreImagen = nombre_imagen;
                        bool rspta = new CN_Reserva().GuardarDatosImagen(oReserva, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardó el banner de reserva pero hubo problemas con la imagen";
                    }
                }
            }


            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oReserva.IdReserva, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //Cadena en base 64//
        [HttpPost]
        public JsonResult ImagenReserva(int id)
        {
            bool conversion;
            Reserva oreserva = new CN_Reserva().Listar().Where(r => r.IdReserva == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oreserva.RutaImagen, oreserva.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textobase64 = textoBase64,
                extension = Path.GetExtension(oreserva.NombreImagen)
            },
            JsonRequestBehavior.AllowGet

            );

        }
        public JsonResult EliminarReserva(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Reserva().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



    }
    #endregion
}