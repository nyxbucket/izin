using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using izin.Data;
using izin.Models;
using izin.Helper;
using static izin.Data.Enum;

namespace izin.Controllers
{
    public class IzinController : BaseController
    {
        private IzinContext izinContext = new IzinContext();

        #region listele
        // GET: Izin
        public ActionResult Index(int? izintipId, DateTime? baslangicTS, DateTime? bitisTS)
        {
            var model = new IzinPageVM();
            model.IzinTipList = IzintipSelection();
            model.IzintipId = izintipId;
            model.BaslangicTS = baslangicTS;
            model.BitisTS = bitisTS;

            var UserId = Convert.ToInt32(User.GetUserPropertyValue("UserId"));

            var query = izinContext.Izinler.Where(x => x.IzinKullaniciId == UserId).AsQueryable().ToList();
            if (izintipId != null)
            {
                query = query.Where(x => x.IzinTipId == izintipId).ToList();
            }
            if (baslangicTS != null)
            {
                query = query.Where(x => x.BaslangicTarihi >= baslangicTS).ToList();
            }
            if (bitisTS != null)
            {
                query = query.Where(x => x.BitisTarihi <= bitisTS).ToList();
            }
            model.IzinlerimList = query;

            return View(model);
        }

        [Authorize(Roles = "Yonetici")]
        public ActionResult IndexOnay(int? kullaniciId, DateTime? baslangicTS, DateTime? bitisTS)
        {
            var model = new IzinPageVM();
            model.TeamUserList = TeamUserSelectList();
            model.UserId = kullaniciId;
            model.BaslangicTS = baslangicTS;
            model.BitisTS = bitisTS;

            var UserId = Convert.ToInt32(User.GetUserPropertyValue("UserId"));
            var depId = izinContext.Departmanlar.FirstOrDefault(x => x.YoneticiKullaniciId == UserId).Id;
            var userList = izinContext.Kullanicilar.Where(x => x.DepartmanId == depId).ToList();

            List<Izin> usersIzin = new List<Izin>();

            if (userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    var query = izinContext.Izinler
                        .Include(x => x.IzinKullanici)
                        .Where(x => x.IzinKullaniciId == item.Id && x.DurumId == 2)
                        .AsQueryable()
                        .ToList();
                    for (int i = 0; i < query.Count; i++)
                    {
                        usersIzin.Add(query[i]);
                    }
                }
            }

            //var query = izinContext.Izinler.Where(x => x.IzinKullaniciId == UserId).AsQueryable().ToList();
            if (kullaniciId != null)
            {
                usersIzin = usersIzin.Where(x => x.IzinKullaniciId == kullaniciId).ToList();
            }
            if (baslangicTS != null)
            {
                usersIzin = usersIzin.Where(x => x.BaslangicTarihi >= baslangicTS).ToList();
            }
            if (bitisTS != null)
            {
                usersIzin = usersIzin.Where(x => x.BitisTarihi <= bitisTS).ToList();
            }
            model.IzinlerimList = usersIzin;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TumIzinler(int? kullaniciId, DateTime? baslangicTS, DateTime? bitisTS)
        {
            var model = new IzinPageVM();            
            model.userList = UserSelectList();
            model.UserId = kullaniciId;
            model.BaslangicTS = baslangicTS;
            model.BitisTS = bitisTS;

            var query = izinContext.Izinler.AsQueryable().ToList();

            if (kullaniciId != null)
            {
                query = query.Where(x => x.IzinKullaniciId == kullaniciId).ToList();
            }
            if (baslangicTS != null)
            {
                query = query.Where(x => x.BaslangicTarihi >= baslangicTS).ToList();
            }
            if (bitisTS != null)
            {
                query = query.Where(x => x.BitisTarihi <= bitisTS).ToList();
            }
            model.IzinlerimList = query;

            return View(model);
        }

        #endregion

        #region metotlar

        public SelectList TeamUserSelectList()
        {
            var UserId = Convert.ToInt32(User.GetUserPropertyValue("UserId"));
            var depId = izinContext.Departmanlar.FirstOrDefault(x => x.YoneticiKullaniciId == UserId).Id;
            var userList = izinContext.Kullanicilar.Where(x => x.DepartmanId == depId).ToList();
            var Kullanicis = userList.Select(x => new { Id = x.Id, Ad = x.AdSoyad }).ToList();
            return new SelectList(Kullanicis, "Id", "Ad");
        }
        public SelectList UserSelectList()
        {
            var Kullanicis = izinContext.Kullanicilar.ToList().Select(x => new { Id = x.Id, Ad = x.AdSoyad }).ToList();
            return new SelectList(Kullanicis, "Id", "Ad");
        }
        public SelectList IzintipSelection()
        {
            var Izintipler = izinContext.IzinTipler.ToList().Select(x => new { Id = x.Id, Aciklama = x.Aciklama }).ToList();

            return new SelectList(Izintipler, "Id", "Aciklama");
        }

        public ActionResult IzinSil(int izinId)
        {
            bool sonuc = true;
            var UserId = User.GetUserPropertyValue("UserId");
            var izin = izinContext.Izinler.FirstOrDefault(x => x.Id == izinId);
            if (izin.IzinKullaniciId != Convert.ToInt32(UserId) || izin.DurumId > 1)
            {
                sonuc = false;
                string res = "Bu işlemi yapmaya yetkili değilsiniz";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            izinContext.Izinler.Remove(izin);
            izinContext.SaveChanges();

            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnayaGonder(int izinId)
        {
            bool sonuc = true;
            var UserId = User.GetUserPropertyValue("UserId");
            var izin = izinContext.Izinler.FirstOrDefault(x => x.Id == izinId);
            if (izin.IzinKullaniciId != Convert.ToInt32(UserId) || izin.DurumId > 1)
            {
                sonuc = false;
                string res = "Bu işlemi yapmaya yetkili değilsiniz";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            izin.DurumId = 2;
            izinContext.Izinler.Attach(izin);
            izinContext.Entry(izin).State = EntityState.Modified;
            izinContext.SaveChanges();

            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Yonetici")]
        public ActionResult Onayla(int id)
        {
            var izin = izinContext.Izinler.FirstOrDefault(x => x.Id == id);
            var UserId = User.GetUserPropertyValue("UserId");
            izin.DurumId = 3;
            izinContext.Izinler.Attach(izin);
            izinContext.Entry(izin).State = EntityState.Modified;
            izinContext.SaveChanges();
            bool sonuc = true;
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Yonetici")]
        public ActionResult Reddet(int id)
        {
            var izin = izinContext.Izinler.FirstOrDefault(x => x.Id == id);
            var UserId = User.GetUserPropertyValue("UserId");
            izin.DurumId = 4;
            izinContext.Izinler.Attach(izin);
            izinContext.Entry(izin).State = EntityState.Modified;
            izinContext.SaveChanges();
            bool sonuc = true;
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region ekle/duzenle

        [HttpGet]
        public ActionResult Ekle(int id = 0)
        {
            var izin = izinContext.Izinler.FirstOrDefault(x => x.Id == id);
            var model = new IzinEkleVM();

            if (izin != null)
            {
                model.Id = izin.Id;
                model.IzinNedeni = izin.IzinNedeni;
                model.IzinTipList = IzintipSelection();
                model.IzinTipId = izin.IzinTipId;
                model.BaslangicTS = izin.BaslangicTarihi;
                model.BitisTS = izin.BitisTarihi;
                model.DurumId = izin.DurumId;
                return View(model);
            }
            model.IzinTipList = IzintipSelection();
            return View(model);
        }

        [HttpPost]
        public ActionResult Ekle(IzinEkleVM model)
        {
            if (ModelState.IsValid)
            {
                var UserId = User.GetUserPropertyValue("UserId");

                if (model.BaslangicTS > model.BitisTS)
                {
                    ViewBag.GenelHata = "Başlangıç tarihi bitiş tarihinden ileri olamaz";
                    model.IzinTipList = IzintipSelection();
                    return View(model);
                }

                if (model.Id > 0)//güncelleme
                {
                    var izin = izinContext.Izinler.FirstOrDefault(x => x.Id == model.Id);
                    if (izin.DurumId != 1)//taslak değilse güncelleyemesin
                    {
                        ViewBag.GenelHata = "Sadece Taslak Durumundaki izinleri güncelleyebilirsiniz";
                        model.IzinTipList = IzintipSelection();
                        return View(model);
                    }
                    izin.IzinNedeni = model.IzinNedeni;
                    izin.BaslangicTarihi = model.BaslangicTS;
                    izin.BitisTarihi = model.BitisTS;
                    izin.IzinTipId = model.IzinTipId;
                    izinContext.Izinler.Attach(izin);
                    izinContext.Entry(izin).State = EntityState.Modified;
                    izinContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    //yeni ekleme
                    var izin = new Izin();
                    izin.IzinNedeni = model.IzinNedeni;
                    izin.BaslangicTarihi = model.BaslangicTS;
                    izin.BitisTarihi = model.BitisTS;
                    izin.IzinTipId = model.IzinTipId;
                    izin.IzinKullaniciId = Convert.ToInt32(UserId);
                    izin.EklemeTarihi = DateTime.Now;
                    if (User.IsInRole("Yonetici") || User.IsInRole("Admin"))
                    {
                        izin.DurumId = 3;//direk onaylanmış olarak kaydedilsin
                    }
                    else
                    {
                        izin.DurumId = 1;//ilk kaydetmede taslak olarak kaydediliyor
                    }

                    izinContext.Izinler.Add(izin);
                    izinContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            model.IzinTipList = IzintipSelection();
            return View(model);
        }

        #endregion
    }
}