using izin.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static izin.Data.Enum;

namespace izin.Models
{    
    public class IzinPageVM 
    {
        //kullanici listesi alt 2
        [Display(Name = "Ad Soyad")]
        public SelectList UserList { get; set; }
        public int? UserId { get; set; }

        //durumlist için alt 2 
        [Display(Name = "Durum")]
        public SelectList DurumList { get; set; }
        [Display(Name = "Durumu")]
        public int? DurumId { get; set; }

        //izintürü için alt 2 
        [Display(Name = "İzin Türü")]
        public SelectList IzinTipList { get; set; }
        [Display(Name = "İzin Türü")]
        public int? IzintipId { get; set; }

        [Display(Name = "İzin Nedeni")]
        public string IzinNedeni { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime? BaslangicTS { get; set; } //= DateTime.Today;

        [Display(Name = "Bitiş Tarihi")]
        public DateTime? BitisTS { get; set; }

        public IPagedList<Izin> Izinler { get; set; }

        public List<Izin> IzinlerimList { get; set; }

        public DateTime EklemeTarihi { get; set; }

        public IzinDurumEnum IzinDurumEnum { get; set; }
        public List<object> IzinDurumEnumList { get; set; }
    }

    public class IzinEkleVM     
    {
        public int Id { get; set; }
        
        [Display(Name = "İzin Türü")]
        public SelectList IzinTipList { get; set; }

        [Display(Name = "İzin Türü")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public int IzinTipId { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public DateTime BaslangicTS { get; set; } = DateTime.Today;

        [Display(Name = "Bitiş Tarihi")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public DateTime BitisTS { get; set; } = DateTime.Today;

        [Display(Name = "İzin Nedeni")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string IzinNedeni { get; set; }

        [Display(Name = "İşlem Tarihi")]
        public DateTime? EklemeTS { get; set; }

        [Display(Name = "Durumu")]
        public int DurumId { get; set; }

    }

}