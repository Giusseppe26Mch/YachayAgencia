using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentaciónAgencia.Controllers
{
 
    //Implemtar Lógica de las vistas y el ingreso de los clientes a la página 
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index() // Método para logear al cliente
        {
            return View();
        }
        public ActionResult Registrar() // Mostrar formulario registro de cliente 
        {
            return View();
        }
        public ActionResult Reestablecer() // Reestablecer contraseña del cliente
        {
            return View();
        }
        public ActionResult CambiarClave() // Cambiar contraseña del cliente
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Cliente objeto) // Cambiar contraseña del cliente
        {
            int resultado;
            string mensaje = string.Empty; // Por defecto le indicamos que será vacío

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres; // Almacenar temporalmente la información que ha ingresado el cliente en el formulario.
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            //En caso las contraseñas coincidan:

            resultado = new CN_Cliente().Registrar(objeto, out mensaje); // out parámetro de salida

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Index(string correo, string clave) // Método para logear al cliente
        {
            Cliente oCliente = null; //new Cliente();

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();



            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();
            }
            else
            {
                //Lógica en caso se haya encontrado las credenciales de ingreso del cliente
                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;//Información de este cliente será llamado

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }

            }

        }

        [HttpPost]
        public ActionResult Reestablecer(string correo) // Reestablecer contraseña del cliente
        {
            Cliente cliente = new Cliente();

            cliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }


            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);
            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmaclave) // Cambiar contraseña del cliente
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";                                 //Permite almacenar valores más simples ( cadenas de texto)//
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();

            }
            else if (nuevaclave != confirmaclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave); //Encriptar nuevamente la contraseña

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index"); //Ingreso
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje; //Mensaje de error de acuerdo a lo que indicamos.
                return View();
            }
             
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut(); //Indicamos que se cierre la sesión.
            return RedirectToAction("Index", "Acceso");
        }
    }

    
}