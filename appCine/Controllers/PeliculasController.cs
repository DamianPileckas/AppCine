using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appCine.Models;
using NPOI.Util;
using Org.BouncyCastle.Utilities.Encoders;

namespace appCine.Controllers
{
    public class peliculasController : Controller
    {
        private cineDBEntities db = new cineDBEntities();

        // GET: peliculas
        public ActionResult Index()
        {
            if (isSession())
            { 
            var usuario = HttpContext.Session["Usuario"];
            var rol = HttpContext.Session["Rol"];
            Console.WriteLine("PRUEBA: " + usuario + " " + rol);
            System.Diagnostics.Debug.WriteLine("PRUEBA: " + usuario + " " + rol);

            return View(db.peliculas.ToList());
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: peliculas/Details/5
        public ActionResult Details(int? id)
        {
            if (isSession())
            { 
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pelicula pelicula = db.peliculas.Find(id);
            if (pelicula == null)
            {
                return HttpNotFound();
            }
            return View(pelicula);
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: peliculas/Create
        public ActionResult Create()
        {
            if (isSession())
            { 
                return View();
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: peliculas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nombre,genero,duracion,director,sinopsis,lanzamiento,categoria,imagen")] pelicula pelicula)
        {
            if (isSession())
            {
                if (ModelState.IsValid)
                {
                    /*string theFileName = Path.GetFileName(pelicula.img.FileName);
                    byte[] thePictureAsBytes = new byte[pelicula.img.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(pelicula.img.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(pelicula.img.ContentLength);
                    }
                    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                    Console.Write("PRUEBA DAMIAN " + thePictureDataAsString);
                    pelicula.imagen = thePictureDataAsString;*/
                    using (var context = new cineDBEntities())
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        InputStream finput = new FileInputStream(file.InputStream);
                        byte[] imageBytes = new byte[(int)file.InputStream.Length];
                        finput.Read(imageBytes, 0, imageBytes.Length);
                        finput.Close();
                        String imageStr = Base64.ToBase64String(imageBytes);
                        var std = new pelicula()
                        {
                            nombre = pelicula.nombre,
                            genero = pelicula.genero,
                            duracion = pelicula.duracion,
                            director = pelicula.director,
                            sinopsis = pelicula.sinopsis,
                            lanzamiento = pelicula.lanzamiento,
                            categoria = pelicula.categoria,
                            imagen = imageStr
                        };
                        context.peliculas.Add(std);
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                        
                    }
                    return RedirectToAction("Index");
                }

                return View(pelicula);
            }
            else
            {
                return RedirectToAction("Login","Login");
            }
        }

        // GET: peliculas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (isSession())
            { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                pelicula pelicula = db.peliculas.Find(id);
                if (pelicula == null)
                {
                    return HttpNotFound();
                }
                return View(pelicula);
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: peliculas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,genero,duracion,director,sinopsis,lanzamiento,categoria,imagen")] pelicula pelicula)
        {
            if (isSession())
            { 
                if (ModelState.IsValid)
            {
                using (var context = new cineDBEntities())
                {
                    var result = db.peliculas.Find(pelicula.Id);
                    if (result != null)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        InputStream finput = new FileInputStream(file.InputStream);
                        byte[] imageBytes = new byte[(int)file.InputStream.Length];
                        finput.Read(imageBytes, 0, imageBytes.Length);
                        finput.Close();
                        String imageStr = Base64.ToBase64String(imageBytes);
                        result.nombre = pelicula.nombre;
                        result.genero = pelicula.genero;
                        result.duracion = pelicula.duracion;
                        result.director = pelicula.director;
                        result.sinopsis = pelicula.sinopsis;
                        result.lanzamiento = pelicula.lanzamiento;
                        result.categoria = pelicula.categoria;
                        result.imagen = imageStr;

                        /*var std = new pelicula()
                        {
                            Id = pelicula.Id,
                            nombre = pelicula.nombre,
                            genero = pelicula.genero,
                            duracion = pelicula.duracion,
                            director = pelicula.director,
                            sinopsis = pelicula.sinopsis,
                            lanzamiento = pelicula.lanzamiento,
                            categoria = pelicula.categoria,
                            imagen = imageStr
                        };*/
                        context.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
            }
            else {
                return RedirectToAction("Login","Login");
            }
        }

    
        // GET: peliculas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (isSession())
            { 
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pelicula pelicula = db.peliculas.Find(id);
            if (pelicula == null)
            {
                return HttpNotFound();
            }
            return View(pelicula);
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (isSession())
            { 
                pelicula pelicula = db.peliculas.Find(id);
            db.peliculas.Remove(pelicula);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            else {
                return RedirectToAction("Login", "Login");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool isSession() {
            return Session["Usuario"] != null && Session["Rol"] != null;
        }
    }
}
