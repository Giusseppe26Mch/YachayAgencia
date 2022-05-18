﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

using System.Web.Security;//Utilizar una referencia 
namespace CapaPresentaciónAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "El correo o la contraseña es incorrecta, ingrese sus credenciales correctamente.";
                return View();

            }
            else

            {
                if (oUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false); //Crear una autenticación de usuario por su correo.

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

            [HttpPost]
            public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";                                 //Permite almacenar valores más simples ( cadenas de texto)//
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();

            }else if (nuevaclave !=confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave); //Encriptar nuevamente la contraseña

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index"); //Ingreso
            }
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje; //Mensaje de error de acuerdo a lo que indicamos.
                return View();
            }
         
        }
    

    [HttpPost]
    public ActionResult Reestablecer(string correo)
    {
        Usuario ousuario = new Usuario();

        ousuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

        if (ousuario == null)
        {
            ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
            return View();
        }


        string mensaje = string.Empty;
        bool respuesta = new CN_Usuarios().ReestablecerClave(ousuario.IdUsuario, correo, out mensaje);
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


        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut(); //Indicamos que se cierre la sesión.
            return RedirectToAction("Index", "Acceso");
        }

    }
}


