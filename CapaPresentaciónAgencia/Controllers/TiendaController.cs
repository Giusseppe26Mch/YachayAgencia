using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

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


        public ActionResult DetalleReserva(int idreserva = 0)
        {
            Reserva oReserva = new Reserva();
            bool conversion;

            oReserva = new CN_Reserva().Listar().Where(r => r.IdReserva == idreserva).FirstOrDefault();
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

        public JsonResult AgregarBolsa (int idreserva)
        {
            //Convertimos esta sesión en un objeto "Cliente"
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_BOLSAVIAJE().ExisteSolicitud(idcliente, idreserva);

            bool respuesta = false; //Por defecto vacío

            string mensaje = string.Empty; //Por defecto vacío

            if (existe)
            {
                mensaje = "La reserva ya ha sido agregada anteriormente";
            }
            else
            {
                respuesta = new CN_BOLSAVIAJE().Operaciones(idcliente, idreserva, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet); 
        }


        [HttpGet]
        public JsonResult CantidadEnBolsa()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_BOLSAVIAJE().CantidadEnBolsa(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }


    }
}