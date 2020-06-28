using izin.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static izin.Data.Enum;

namespace izin.Models
{   
    public class KullaniciVM
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }

        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Display(Name = "Eposta")]
        public string eposta { get; set; }

        [Display(Name = "Şifre")]
        public string Parola { get; set; }

        [Display(Name = "Departman")]
        public int? DepartmanId { get; set; }
        
        [Display(Name = "Aktif Mi")]
        public bool AktifMi { get; set; }

        [Display(Name = "Roller")]
        public List<Rol> RolList { get; set; }

        public List<Kullanici> KullaniciList { get; set; }

        [Display(Name = "Roller")]
        public string Rol { get; set; }

        [Display(Name = "Roller")]
        public int? RolId { get; set; }

        public int? IsRolVal { get; set; }

        public RolEnum RolEnum { get; set; }
        public List<object> RolEnumList { get; set; }


    }


    public class KullaniciEkleVM
    {
        public int? id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string KullaniciAdi { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string AdSoyad { get; set; }

        [Display(Name = "Eposta")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string eposta { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string Parola { get; set; }

        [Display(Name = "Aktif Mi")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public bool AktifMi { get; set; }

        [Display(Name = "Admin")]
        public bool AdminMi { get; set; }

        [Display(Name = "Yönetici")]
        public bool YoneticiMi { get; set; }

        [Display(Name = "Personel")]
        public bool PersonelMi { get; set; }




    }

}