using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace izin.Data
{
    public class Enum
    {
        public enum RolEnum
        {
            [Description("Hepsi")]
            Hepsi = 1,
            [Description("Admin")]
            Admin = 2,
            [Description("Yonetici")]
            [Display(Name = "Yönetici")]
            Yonetici = 3,
            [Description("Personel")]
            Personel = 4,

        }

        public enum IzinDurumEnum
        {
            [Description("Taslak")]
            [Display(Name = "Taslak")]
            Taslak = 1,

            [Description("Onay Bekliyor")]
            [Display(Name = "Onay Bekliyor")]
            OnayBekliyor = 2,

            [Description("Onaylandi")]
            [Display(Name = "Onaylandı")]
            Onaylandi = 3,

            [Description("Reddedildi")]
            [Display(Name = "Reddedildi")]
            Reddedildi = 4
        }

        public enum IzinTipEnum
        {
            [Description("Saatlik")]
            [Display(Name = "Saatlik")]
            Saatlik = 1,

            [Description("Günlük")]
            [Display(Name = "Günlük")]
            Gunluk = 2

        }

    }
}