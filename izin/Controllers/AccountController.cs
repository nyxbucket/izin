using izin.Data;
using izin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.Configuration;
using izin.Helper;

namespace izin.Controllers
{
    public class AccountController : BaseController
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;

            }
        }
        //db connection
        private IzinContext izinContext = new IzinContext();

        [AllowAnonymous]
        public ActionResult Navigate()
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.GetUserPropertyValue("UserId");

                if (UserId != null)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Kullanici");
                    }
                    if (User.IsInRole("Yonetici") || User.IsInRole("Personel"))
                    {
                        return RedirectToAction("Index", "Izin");
                    }
                }
            }
            return RedirectToAction("LogOff");
        }

        //[HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            string username = model.UserName.Trim();
            string pwd = model.Password.Trim();

            Kullanici user = new Kullanici();

            user = izinContext.Kullanicilar.Where(x => x.KullaniciAdi == model.UserName && x.Parola == model.Password).FirstOrDefault();
            if (user != null)
            {
                if (user.AktifMi == true)
                {
                    List<Rol> userRol = new List<Rol>();
                    userRol = user.Roller.ToList();
                    if (userRol != null)
                    {
                        List<Claim> claims = new List<Claim>();

                        for (int i = 0; i < userRol.Count; i++)
                        {
                            if (userRol[i].Id == 1)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                            }
                            else if (userRol[i].Id == 2)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "Yonetici"));
                            }
                            else if (userRol[i].Id == 3)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "Personel"));
                            }
                            else
                            {
                                //do make hata  ekle
                                return View(new LoginViewModel());
                            }
                        }

                        var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                        identity.AddClaim(new Claim("UserId", user.Id.ToString()));
                        identity.AddClaim(new Claim("UserName", user.KullaniciAdi.ToString()));
                        identity.AddClaim(new Claim("AdSoyad", user.AdSoyad.ToString()));

                        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
                        Session["userName"] = user.AdSoyad;
                        //do make navigation

                        //return RedirectToAction("Index", "Kullanici");
                        return RedirectToAction("Navigate");
                    }

                }
            }
            else
            {
                model.LoginErrorMessage = "Yanlış Kullanıcı veya şifre";
                return View("Login", model);

            }

            return View(model);
        }


        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login", "Account");
        }


    }
}