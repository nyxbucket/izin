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
                query = query.Where(x => x.BaslangicTarihi <= baslangicTS).ToList();
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

        public SelectList IzintipSelection()
        {
            var Izintipler = izinContext.IzinTipler.ToList().Select(x => new { Id = x.Id, Aciklama = x.Aciklama }).ToList();

            return new SelectList(Izintipler, "Id", "Aciklama");
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
                var izin = new Izin();

            }
            model.IzinTipList = IzintipSelection();
            return View(model);
        }

        #endregion
    }
}