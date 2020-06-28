using izin.Data;
using izin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Script.Serialization;

namespace izin.Controllers
{
    public class DepartmanController : BaseController
    {
        private IzinContext izinContext = new IzinContext();
        #region liste
        // GET: Departman
        public ActionResult Index(string ad, int? yoneticiKullaniciId)
        {
            var model = new DepartmanPageVM();
            model.YoneticiKullaniciId = yoneticiKullaniciId;
            model.Ad = ad;

            var tlList = izinContext.Departmanlar.Select(x => x.YoneticiKullanici).Distinct().ToList();
            tlList.ForEach(x => x.AdSoyad = x.AdSoyad);
            model.TakimLideriList = tlList;
            var query = izinContext.Departmanlar.OrderByDescending(x => x.Id).AsQueryable().ToList();
            if ((ad == "" || ad == null) && yoneticiKullaniciId == null)
            {
                model.DprtmnList = query;
            }
            else if (yoneticiKullaniciId != null)
            {
                query = query.Where(x => x.YoneticiKullaniciId==yoneticiKullaniciId).ToList();
                
            }
            else if(ad != null && ad != "")
            {
                query = query.Where(x => x.Ad.Contains(ad)).ToList();
            }
            model.DprtmnList = query;

            //var response = Proxy.Call((IEkipService s) => s.Get(req));


            return View(model);
        }
        #endregion

        #region metotlar

        public SelectList DepartmanSelectList()
        {
            var Ekipler = izinContext.Departmanlar.ToList().Select(x => new { Id = x.Id, Ad = x.Ad }).ToList();

            return new SelectList(Ekipler, "Id", "Ad");
        }
        public ActionResult DepartmanSil(int ekipId)
        {
            var userControl = izinContext.Kullanicilar.Where(x=>x.DepartmanId==ekipId).ToList().Count(); 
            string res = "";
            if (userControl > 0)
            {
                res = "Departman silebilmek için lütfen önce bu departmanın üyelerini siliniz";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            if (userControl == 0)
            {
                var depS = izinContext.Departmanlar.FirstOrDefault(i => i.Id == ekipId);
                if (depS != null)
                {
                    var teamUsers = izinContext.Kullanicilar.Where(x => x.DepartmanId == ekipId);
                    foreach (var users in teamUsers)
                    {
                        users.DepartmanId = null;
                    }
                    izinContext.Departmanlar.Remove(depS);
                    izinContext.SaveChanges();
                }
            }
            bool sonuc = true;
            //res = "Silme işlemi başarılı";
            return Json(sonuc, JsonRequestBehavior.AllowGet);
            //return true;
        }

        #endregion


        #region ekle/duzenle
        [HttpGet]
        public ActionResult Ekle(int id = 0)
        {
            var model = new DepartmanJsonVM();

            var teams = izinContext.Departmanlar.FirstOrDefault(x => x.Id == id); 
            if (teams != null)
            {
                model.Ad = teams.Ad;
                model.YonId = teams.YoneticiKullaniciId;
                model.Id = teams.Id;

                List<Kullanici> validUsers = new List<Kullanici>();
                var dpusersList = izinContext.Kullanicilar.Where(x=>x.DepartmanId==id).ToList();
                
                for (int i = 0; i < dpusersList.Count; i++)
                {
                    Kullanici usr = new Kullanici();
                    usr.Id = dpusersList[i].Id ;
                    usr.AdSoyad = dpusersList[i].AdSoyad;
                    validUsers.Add(usr);
                }
                model.DepartmanKullaniciList = validUsers;
            }
            model.Id = id;

            var query = izinContext.Roller.Include(x => x.Kullanicilar).FirstOrDefault(x => x.Id ==2);//yönetici olan kullanıcıları çek
            var yoneticiList = query.Kullanicilar.Where(x => x.AktifMi == true).ToList();
            yoneticiList.ForEach(x => x.AdSoyad = x.AdSoyad);
            model.YoneticiList = yoneticiList;

            var kullaniciList = izinContext.Kullanicilar.Where(x=>x.AktifMi==true&&x.DepartmanId==null).ToList();            
            model.KullaniciList = kullaniciList;

            //model.KullaniciList = kulList;
            return View(model);
        }



        [HttpPost]
        public JsonResult DepartmanEkle(DepartmanJsonVM departman)
        {
            string res = "";
            if (ModelState.IsValid)
            {
                var _Departadi = izinContext.Departmanlar.Where(x => x.Ad == departman.Ad).FirstOrDefault();
                var dep = new Departman();
                if (departman.Id == 0)
                {                    
                    if (_Departadi == null)
                    {                        
                        dep.Ad = departman.Ad;
                        dep.YoneticiKullaniciId = departman.YonId;

                        izinContext.Departmanlar.Add(dep);
                        var depUsers = izinContext.Kullanicilar.Where(x => departman.KullaniciIdList.Contains(x.Id));

                        if (departman.KullaniciIdList != null)
                        {
                            foreach (var users in depUsers)
                            {
                                users.DepartmanId = dep.Id;
                            }
                            
                        }
                        izinContext.SaveChanges();
                    }
                    else
                    {
                        res = "Bu Departmanın Adı Daha Önce Kullanılmıştır";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {                    
                    var varolanAd = izinContext.Departmanlar.FirstOrDefault(x=>x.Id== departman.Id).Ad;
                    if (varolanAd == departman.Ad || _Departadi == null)
                    {
                        var depExist = izinContext.Departmanlar.FirstOrDefault(x => x.Id == departman.Id);
                        var oldUsers = izinContext.Kullanicilar.Where(x => departman.Id == x.DepartmanId).ToList();

                        if (departman.KullaniciIdList == null)
                        {
                            foreach (var del_users in oldUsers)
                            {
                                del_users.DepartmanId = null;
                            }
                        }
                        if (departman.KullaniciIdList != null)
                        {
                            var newUsers = izinContext.Kullanicilar.Where(x => departman.KullaniciIdList.Contains(x.Id));
                            var ListelerArasiFark = oldUsers.Except(newUsers).ToList();

                            foreach (var New_users in newUsers)
                            {
                                New_users.DepartmanId = departman.Id;
                            }

                            foreach (var Del_users in ListelerArasiFark)
                            {
                                Del_users.DepartmanId = null;
                            }
                        }
                        depExist.YoneticiKullaniciId = departman.YonId;
                        depExist.Ad = departman.Ad;
                        izinContext.Departmanlar.Attach(depExist);
                        izinContext.Entry(depExist).State = EntityState.Modified;
                        izinContext.SaveChanges();
                    }
                    else
                    {
                        res = "Bu Departmanın Adı Daha Önce Kullanılmıştır";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                if (departman.Id > 0)
                {
                    bool guncellemeSonucu = true;
                    return Json(guncellemeSonucu, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool eklemeSonucu = true;
                    return Json(eklemeSonucu, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                res = "Departmanın Ad ve Yöneticisi Alanlarını Kontrol Ediniz.";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}