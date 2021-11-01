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
{ 
    [Authorize]
    public class HomeController : Controller
    {
        otovrtEntities db = new otovrtEntities();
        [HttpGet]
        public ActionResult Biletler()
        {
            var kullanicilar = Request.Cookies["kullanicilar"];
            if (kullanicilar["ydurum"] != null)
            {
                if (kullanicilar["ydurum"] == "1")
                {
                    Response.Redirect("/Home/BiletlerDuzenle");
                }
            }
            List<guzergah> guzergahlar = db.guzergah.ToList();

            return View(guzergahlar);
        }
        public ActionResult BiletlerDuzenle()
        {
            var kullanicilar = Request.Cookies["kullanicilar"];
            if (kullanicilar["ydurum"] != null)
            {
                if (kullanicilar["ydurum"] == "0")
                {
                    Response.Redirect("/Home/Biletler");
                }
            }
            List<bilet> biletler = db.bilet.ToList();

            return View(biletler);
        }
        public ActionResult BiletSil(int id)
        {
            var kullanicilar = Request.Cookies["kullanicilar"];
            if (kullanicilar["ydurum"] != null)
            {
                if (kullanicilar["ydurum"] == "0")
                {
                    Response.Redirect("/Home/Biletler");
                }
            }
            var bilets = db.bilet.Find(id);
            var guzergah = db.guzergah.Where(x => x.id == bilets.guzergahid).ToList();
            foreach (var x in guzergah)
            {
                x.bkoltuk = x.bkoltuk + 1;
            }
            db.bilet.Remove(bilets);
            db.SaveChanges();
            return RedirectToAction("BiletlerDuzenle");
        }
        public ActionResult BiletAl(string id)
        {
            int gid = Convert.ToInt32(id);
            if (id!=null)
            {
                var kullanicilar = Request.Cookies["kullanicilar"];
            if (kullanicilar["ydurum"] != null)
            {
                if (kullanicilar["ydurum"] == "1")
                {
                    Response.Redirect("/Home/BiletlerDuzenle");
                }
            }
            List<string> boskoltuklar = new List<string>();
            var guzergah = db.guzergah.Find(gid);
            var bskltk = db.bilet.Where(x => x.guzergahid == gid).ToList();
            for (int i = 1; i <= 20; i++)
            {
                boskoltuklar.Add(Convert.ToString(i));
            }
            foreach (var x in bskltk)
            {
                for (int i = 1; i <= 20; i++)
                {
                    if (i == x.koltukno)
                    {
                        boskoltuklar.Remove(Convert.ToString(i));
                        break;
                    }
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 0; i < boskoltuklar.Count; i++)
            {
                items.Add(new SelectListItem { Text = boskoltuklar[i], Value = boskoltuklar[i] });
            }
                ViewBag.ucret = guzergah.ucret;
                ViewBag.bskoltuk = items;
                return View();
            }
            return Redirect("/Home/Biletler");

            
        }
        [HttpPost]
        public ActionResult BiletAl(bilet fc, int id)
        {
            var kullanicilar = Request.Cookies["kullanicilar"];
            if (kullanicilar["ydurum"] != null)
            {
                if (kullanicilar["ydurum"] == "1")
                {
                    Response.Redirect("/Home/BiletlerDuzenle");
                }
            }
            List<string> boskoltuklar = new List<string>();
            var bskltk = db.bilet.Where(x => x.guzergahid == id).ToList();
            var guzergahguncelle = db.guzergah.Find(id);
            var blt = db.bilet.ToList();
            int biletsay = 0;
            foreach (var x in blt)
            {
                if (x.guzergahid == id)
                {
                    biletsay++;
                }
            }
            int zamsay = Convert.ToInt32(guzergahguncelle.zamsay);
            int zam = (biletsay / 5) - zamsay;
            double ucret = Convert.ToDouble(guzergahguncelle.ucret);
            double zamc = 0;
            for (int i = 1; i <= zam; i++)
            {
                zamc = ucret * 0.10;
            }
            ucret = ucret+zamc;
            fc.ucret = ucret;
            fc.guzergahid = id;
            fc.guzergahadi = guzergahguncelle.guzergahad;
            db.bilet.Add(fc);
            for (int i = 1; i <= 20; i++)
            {
                boskoltuklar.Add(Convert.ToString(i));
            }
            foreach (var x in bskltk)
            {
                for (int i = 1; i <= 20; i++)
                {
                    if (i == x.koltukno)
                    {
                        boskoltuklar.Remove(Convert.ToString(i));
                        break;
                    }
                }
            }
            int kltsy=0;
            for (int i = 1; i <= boskoltuklar.Count; i++)
            {
                kltsy++;
            }
            guzergahguncelle.bkoltuk = kltsy-1;
            guzergahguncelle.ucret = ucret;
            guzergahguncelle.zamsay = guzergahguncelle.zamsay + zam;
            db.SaveChanges();
            return Redirect("/Home/Biletler");
        }

    }
}