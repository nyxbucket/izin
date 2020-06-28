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
    public class DepartmanVM 
    {
        public int Id { get; set; }

        [Display(Name = "Departman")]
        public string Ad { get; set; }

        [Display(Name = "Yöneticisi")]
        public int? YoneticiKullaniciId { get; set; }
    }

    public class DepartmanPageVM
    {
        //public int Id { get; set; }

        public int? DepartmanId { get; set; }

        [Display(Name = "Departman Adı")]
        public string Ad { get; set; }

        [Display(Name = "Yöneticisi")]
        public int? YoneticiKullaniciId { get; set; }

        public IPagedList<Departman> Departmanlar { get; set; }

        [Display(Name = "Yöneticisi")]
        public List<Kullanici> TakimLideriList { get; set; }

        [Display(Name = "Departman Adı")]
        public SelectList DepartmanList { get; set; }

        [Display(Name = "Departman Listesi")]
        public List<Departman> DprtmnList { get; set; }
    }

    public class DepartmanJsonVM 
    {
        [Display(Name = "Departman Adı")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public int? YonId { get; set; }

        public int[] KullaniciIdList { get; set; }

        [Display(Name = "Departman Yöneticisi")]
        public List<Kullanici> YoneticiList { get; set; }

        [Display(Name = "Departman Üyesi")]
        public List<Kullanici> KullaniciList { get; set; }

        public List<Kullanici> DepartmanKullaniciList { get; set; }

        public int Id { get; set; }

        public string jsonDpList { get; set; }
    }


}