using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Projek.Models;
using System.Web.Security;

namespace Projek.Controllers
{[Authorize]
    public class UsersController : Controller
    {
        otovrtEntities db = new otovrtEntities();
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }
        [AllowAnonymous]
        
        [HttpPost]
        public ActionResult Giris(kullanici p)
        {
            var bilgiler = db.kullanici.FirstOrDefault(x => x.mail == p.mail && x.sifre == p.sifre);
            
            if (bilgiler != null)
            {
                HttpCookie kullanicilar = new HttpCookie("kullanicilar");
                FormsAuthentication.RedirectFromLoginPage(p.mail, false);
                kullanicilar["ydurum"]= Convert.ToString(bilgiler.ydurum);
                Response.Cookies.Add(kullanicilar);
                return RedirectToAction("Biletler", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya Parola Hatalı!";
                return View();
            }

        }
        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Biletler", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Kayitol()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Kayitol(kullanici s)
        {
            if (s.mail != null && s.sifre != null)
            {
                s.ydurum = 0;
                db.kullanici.Add(s);
                db.SaveChanges();
                HttpCookie kullanicilar = new HttpCookie("kullanicilar");
                FormsAuthentication.RedirectFromLoginPage(s.mail, false);
                kullanicilar["ydurum"] = Convert.ToString(s.ydurum);
                Response.Cookies.Add(kullanicilar);
                return RedirectToAction("Biletler", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya Parola Hatalı!";
                return View();
            }

        }
    }
}