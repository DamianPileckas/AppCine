using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appCine.Models;
using System.Text;
using System.Security.Cryptography;

namespace appCine.Controllers
{
    public class LoginController : Controller
    {
        private cineDBEntities1 db = new cineDBEntities1();

        // GET: Login
        public ActionResult Index()
        {
            return View(db.usuarios.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            if (isSession())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            
        }

        // POST: Login/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usuario1,password,rol")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                using (var context = new cineDBEntities1())
                {
                    var user = new usuario()
                    {
                        usuario1 = usuario.usuario1,
                        password = usuario.password,
                        rol = "admin",
                    };
                    context.usuarios.Add(user);
                    context.SaveChanges();
                    return RedirectToAction("Index","Home");
                }
            }

            return View(usuario);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Login/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,usuario1,password,rol")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuario usuario = db.usuarios.Find(id);
            db.usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string usuario1, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var loginUser = db.usuarios.Where(s => s.usuario1.Equals(usuario1) && s.password.Equals(password)).FirstOrDefault();

                if (loginUser == null)
                {
                    ModelState.AddModelError("usuario1", " usuario o password incorrectos");
                    return RedirectToAction("Login");
                }
                else
                {
                    Session.Add("Usuario", loginUser.usuario1.ToString());
                    Session.Add("Rol", loginUser.rol.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }



        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool isSession()
        {
            return Session["Usuario"] != null && Session["Rol"] != null;
        }
    }
}
