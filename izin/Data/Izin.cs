using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services.Description;

namespace izin.Data
{
    [Table("Izin")]
    [DataContract(IsReference = true)]
    public class Izin
    {
        [DataMember]
        [Editable(false)]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string IzinNedeni { get; set; }

        [DataMember]
        [Required]
        public DateTime BaslangicTarihi { get; set; }

        [DataMember]
        [Required]
        public DateTime BitisTarihi { get; set; }

        [DataMember]
        [Required]
        public int IzinTipId { get; set; }

        [DataMember]
        [ForeignKey("IzinTipId")]
        public virtual IzinTip IzinTip { get; set; }

        [DataMember]
        [Required]
        public DateTime EklemeTarihi { get; set; }

        [DataMember]
        [Required]
        public int IzinKullaniciId { get; set; }

        [DataMember]
        [ForeignKey("IzinKullaniciId")]
        public virtual Kullanici IzinKullanici { get; set; }

        [DataMember]
        [Required]
        public int DurumId { get; set; }

        [ForeignKey("DurumId")]
        public virtual Durum Durum { get; set; }
    }
}