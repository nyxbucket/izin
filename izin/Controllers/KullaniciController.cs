using izin.Data;
using izin.Helper;
using izin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using static izin.Data.Enum;

namespace izin.Controllers
{
    public class KullaniciController : BaseController
    {
        //db connection
        private IzinContext izinContext = new IzinContext();

        #region listeleme
        // GET: Kullanici
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string adSoyad, int? isrolVal)
        {
            #region deneme
            //var abc = User;
            //var sa = User.IsInRole("Admin");

            //var user = new Kullanici();
            //user = model.KullaniciList[0];
            #endregion

            var model = new KullaniciVM();
            model.AdSoyad = adSoyad;
            model.IsRolVal = isrolVal;
            model.RolEnumList = EnumHelper.GetEnumDescWithVal<RolEnum>().ToList<object>();
            //filtre alanları temiz iken
            var query = izinContext.Kullanicilar.Include(x => x.Roller).OrderByDescending(x => x.Id).AsQueryable().ToList(); 
            if (adSoyad==null&&(isrolVal ==null|| isrolVal == 1))
            {
                model.KullaniciList = query;
            }
            else if(isrolVal != null)
            {
                List<Rol> rolList = new List<Rol>();
                List<Kullanici> kullaniciList = new List<Kullanici>();
                if(isrolVal ==null || isrolVal == 1)
                {
                    model.KullaniciList = query.Where(x => x.AdSoyad.Contains(adSoyad)).ToList();
                    return View(model);
                }
                else if(isrolVal == 2)//adminleri
                {                    
                    for (int i = 0; i < query.Count; i++)
                    {
                        rolList = query[i].Roller.ToList();
                        var adminRol = rolList.Where(x => x.RolAdi == "Admin").ToList();
                        if (adminRol != null && adminRol.Count>0)
                        {
                            kullaniciList.Add(query[i]);
                        }
                    }
                }
                else if (isrolVal == 3)//yonetici
                {
                    for (int i = 0; i < query.Count; i++)
                    {                        
                        rolList = query[i].Roller.ToList();
                        var yoneticiRol = rolList.Where(x => x.RolAdi == "Yonetici").ToList();
                        if (yoneticiRol != null && yoneticiRol.Count>0)
                        {
                            kullaniciList.Add(query[i]);
                        }
                    }
                }
                else if (isrolVal == 4)//personel
                {
                    for (int i = 0; i < query.Count; i++)
                    {
                        rolList = query[i].Roller.ToList();
                        var personelRol = rolList.Where(x => x.RolAdi == "Personel").ToList();
                        if (personelRol != null && personelRol.Count>0)
                        {
                            kullaniciList.Add(query[i]);
                        }
                    }
                }                
                if(adSoyad != null && adSoyad!="")
                {
                    kullaniciList = query.Where(x => x.AdSoyad.Contains(adSoyad)).ToList();
                }
                model.KullaniciList = kullaniciList;
            }

            //model.RolList = izinContext.Roller.ToList();

            model.RolEnumList = EnumHelper.GetEnumDescWithVal<RolEnum>().ToList<object>();

            return View(model);
        }
        #endregion


        #region ekle/duzenle
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Ekle(int id = 0)
        {
            var model = new KullaniciEkleVM();
            var user = izinContext.Kullanicilar.Where(x => x.Id == id).FirstOrDefault();
            //düzenleme işlemi oluyor
            if (user != null)
            {
                model.id = user.Id;
                model.KullaniciAdi = user.KullaniciAdi;
                model.AdSoyad = user.AdSoyad;
                model.eposta = user.Eposta;
                model.Parola = user.Parola;
                model.AktifMi = user.AktifMi;

                List<Rol> userRol = new List<Rol>();
                userRol = user.Roller.ToList();
                for (int i = 0; i < userRol.Count; i++)
                {
                    if (userRol[i].RolAdi == "Admin")
                    {
                        model.AdminMi = true;
                    }
                    else if (userRol[i].RolAdi == "Yonetici")
                    {
                        model.YoneticiMi = true;
                    }
                    else if (userRol[i].RolAdi == "Personel")
                    {
                        model.PersonelMi = true;
                    }
                }
            }
            //elsesi yada 0 dan kullanıcı ekliyoruz
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Ekle(KullaniciEkleVM model)
        {
            if (ModelState.IsValid)
            {
                var user = izinContext.Kullanicilar.Where(x => x.KullaniciAdi == model.KullaniciAdi).FirstOrDefault();
                //yeni kullanıcımı? sorusuna cevap arıyoruz
                if (user == null || (user != null && user.Id == 0))
                {
                    #region kullanıcı ekleme için bilgiler set ediliyor
                    var newUser = new Kullanici();
                    newUser.KullaniciAdi = model.KullaniciAdi;
                    newUser.Parola = model.Parola;
                    newUser.AdSoyad = model.AdSoyad;
                    newUser.Eposta = model.eposta;
                    newUser.AktifMi = true;

                    //rolleri ekliyoruz
                    newUser.Roller = new List<Rol>();
                    var adminRol = izinContext.Roller.FirstOrDefault(x => x.Id == 1);
                    var yoneticiRol = izinContext.Roller.FirstOrDefault(x => x.Id == 2);
                    var personelRol = izinContext.Roller.FirstOrDefault(x => x.Id == 3);
                    if (model.AdminMi)
                    {
                        newUser.Roller.Add(adminRol);
                    }
                    if (model.YoneticiMi)
                    {
                        newUser.Roller.Add(yoneticiRol);
                    }
                    if (model.PersonelMi)
                    {
                        newUser.Roller.Add(personelRol);
                    }
                    izinContext.Kullanicilar.Add(newUser);
                    izinContext.SaveChanges();

                    return RedirectToAction("Index");
                    #endregion
                }
                else
                {
                    if (model.id > 0)
                    {
                        #region kullanıcı düzenleme işlemi için bilgiler set ediliyor

                        user.AdSoyad = model.AdSoyad;
                        user.KullaniciAdi = model.KullaniciAdi;
                        user.Parola = model.Parola;
                        user.Eposta = model.eposta;
                        user.AktifMi = model.AktifMi;

                        user.Roller = new List<Rol>();
                        var adminRol = izinContext.Roller.FirstOrDefault(x => x.Id == 1);
                        var yoneticiRol = izinContext.Roller.FirstOrDefault(x => x.Id == 2);
                        var personelRol = izinContext.Roller.FirstOrDefault(x => x.Id == 3);

                        if (model.AdminMi)
                        {
                            user.Roller.Add(adminRol);
                        }
                        else
                        {
                            user.Roller.Remove(adminRol);
                        }
                        if (model.YoneticiMi)
                        {
                            user.Roller.Add(yoneticiRol);
                        }
                        else
                        {
                            user.Roller.Remove(yoneticiRol);
                        }
                        if (model.PersonelMi)
                        {
                            user.Roller.Add(personelRol);
                        }
                        else
                        {
                            user.Roller.Remove(personelRol);
                        }

                        izinContext.Kullanicilar.Attach(user);
                        izinContext.Entry(user).State = EntityState.Modified;
                        izinContext.SaveChanges();
                        #endregion

                        return RedirectToAction("Index");
                    }
                }

            }
            return View(model);
        }

        #endregion

        #region metotlar

        public ActionResult KullaniciSil(int kullaniciId)
        {
            Kullanici user = izinContext.Kullanicilar.Find(kullaniciId);
            user.AktifMi = false;
            izinContext.Kullanicilar.Attach(user);
            izinContext.Entry(user).State = EntityState.Modified;
            izinContext.SaveChanges();

            bool islemDurum = true;
            return Json(islemDurum, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}