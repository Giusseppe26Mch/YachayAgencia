﻿using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System;

namespace CapaPresentaciónAgencia.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PolíticasAgencia()
        {
            return View();
        }
        public ActionResult TérminosAgencia()
        {
            return View();
        }
        public ActionResult QuienesSomos()
        {
            return View();
        }
        public ActionResult Contáctanos()
        {
            return View();
        }


        public ActionResult DetalleReserva(int idreserva = 0)
        {
            Reserva oReserva = new Reserva();
            bool conversion;

            oReserva = new CN_Reserva().Listar().Where(r => r.IdReserva ==idreserva).FirstOrDefault();
            if (oReserva != null)
            {
                oReserva.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oReserva.RutaImagen, oReserva.NombreImagen), out conversion);
                oReserva.Extension = Path.GetExtension(oReserva.NombreImagen);
            }

            return View(oReserva);
        }

        //Método que devuelve la lista de las categorías 
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarPaqueteporCategoria(int idcategoria)
        {
            List<PaqueteTuristico> lista = new List<PaqueteTuristico>();
            lista = new CN_Paquete().ListarPaqueteporCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        /*Método para listar las reservas de acuerdo a una categoría o paquete seleccionado(filtro)*/
        [HttpPost]
        public JsonResult ListarReserva(int idcategoria, int idpaquete)
        {
            List<Reserva> lista = new List<Reserva>();
            bool conversion;

            lista = new CN_Reserva().Listar().Select(r => new Reserva()
            {
                IdReserva = r.IdReserva,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                oPaquete = r.oPaquete,
                ocategoria = r.ocategoria,
                Precio = r.Precio,
                Stock = r.Stock,
                RutaImagen = r.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(r.RutaImagen, r.NombreImagen), out conversion),
                Extension = Path.GetExtension(r.NombreImagen),
                Activo = r.Activo
            }).Where(r =>
            r.ocategoria.IdCategoria == (idcategoria == 0 ? r.ocategoria.IdCategoria : idcategoria) &&
            r.oPaquete.IdPaquete == (idpaquete == 0 ? r.oPaquete.IdPaquete : idpaquete) &&
            r.Stock > 0 && r.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            /*No tendrá un límite en su contenido con lo siguiente: */
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;


        }


        [HttpPost]

        public JsonResult AgregarBolsa(int idreserva)
        {
            //Convertimos esta sesión en un objeto "Cliente"
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_BolsaViaje().ExisteSolicitud(idcliente, idreserva);

            bool respuesta = false; //Por defecto vacío

            string mensaje = string.Empty; //Por defecto vacío

            if (existe)
            {
                mensaje = "La reserva ya ha sido agregada anteriormente!!";
            }
            else
            {
                respuesta = new CN_BolsaViaje().OperacionBolsaviaje(idcliente, idreserva, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadEnBolsa()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_BolsaViaje().CantidadEnBolsa(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ListarReservasBolsa()
        {

            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            List<Bolsadeviaje> oLista = new List<Bolsadeviaje>();

            bool conversion;

            oLista = new CN_BolsaViaje().ListarReserva(idcliente).Select(oc => new Bolsadeviaje()
            {
                oReserva = new Reserva()
                {
                    IdReserva = oc.oReserva.IdReserva,
                    Nombre = oc.oReserva.Nombre,
                    oPaquete = oc.oReserva.oPaquete,
                    Precio = oc.oReserva.Precio,
                    RutaImagen = oc.oReserva.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oReserva.RutaImagen, oc.oReserva.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oReserva.NombreImagen)

                },
                Cantidad = oc.Cantidad


            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult OperacionBolsaviaje(int idreserva, bool sumar)
        {
            //Convertimos esta sesión en un objeto "Cliente"
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;


            bool respuesta = false; //Por defecto vacío

            string mensaje = string.Empty; //Por defecto vacío

            respuesta = new CN_BolsaViaje().OperacionBolsaviaje(idcliente, idreserva, true, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Eliminarviaje(int idreserva)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;


            bool respuesta = false; //Por defecto vacío

            string mensaje = string.Empty; //Por defecto vacío

            respuesta = new CN_BolsaViaje().Eliminarviaje(idcliente, idreserva);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();

            oLista = new CN_Ubicacion().ObtenerDepartamento();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerProvincia(string IdDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();

            oLista = new CN_Ubicacion().ObtenerProvincia(IdDepartamento);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerDistrito(string IdDepartamento, string IdProvincia)
        {
            List<Distrito> oLista = new List<Distrito>();

            oLista = new CN_Ubicacion().ObtenerDistrito(IdDepartamento, IdProvincia);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult ObtenerFechasTour()
        {
            List<Tour> oLista = new List<Tour>();

            oLista = new CN_Ubicacion().ObtenerFechasTour();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Bolsa()
        {
            return View();
        }

        [HttpPost]
        //Asíncrono
        public async Task<JsonResult> ProcesarPago(List<Bolsadeviaje> oListaBolsadeviaje, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-PE");
            detalle_venta.Columns.Add("IdReserva", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));
   
            //Iterar la lista de la bolsa, y así mantener el almacenamiento temporal

            foreach(Bolsadeviaje oBolsadeviaje in oListaBolsadeviaje)
            {
                decimal subtotal = Convert.ToDecimal(oBolsadeviaje.Cantidad.ToString()) * oBolsadeviaje.oReserva.Precio;

                total += subtotal;

                detalle_venta.Rows.Add(new object[]
                {
                    oBolsadeviaje.oReserva.IdReserva,
                    oBolsadeviaje.Cantidad,
                    subtotal
                });

            }

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            //Pago realizado recibe 2 parámetros: idTransaccion y el estado
            return Json(new { Status = true, Link = "/Tienda/PagoRealizado?idtransaccion=code0001&status=true" }, JsonRequestBehavior.AllowGet);

        }


        //Método que nos devolverá esa vista

        public async Task<ActionResult> PagoRealizado()
        {
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean(Request.QueryString["status"]);

            ViewData["Status"] = status;

            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];

                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                string mensaje = string.Empty;

                oVenta.IdTransaccion = idtransaccion;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            
            }

            return View();
        }

    }
}