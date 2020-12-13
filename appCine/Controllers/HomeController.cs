using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appCine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (isSession())
            {
                ViewBag.Message = "Hola "+Session["Usuario"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult About()
        {
            if (isSession())
            {
                ViewBag.Message = "Your application description page.";

            return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Contact()
        {
            if (isSession())
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        private bool isSession()
        {
            return Session["Usuario"] != null && Session["Rol"] != null;
        }
    }
}