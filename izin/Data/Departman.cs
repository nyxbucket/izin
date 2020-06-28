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
    [Table("Departman")]
    [DataContract(IsReference = true)]
    public class Departman
    {
        [DataMember]
        [Editable(false)]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Ad { get; set; }

        [DataMember]
        [Required]
        public int? YoneticiKullaniciId { get; set; }

        [DataMember]
        [ForeignKey("YoneticiKullaniciId")]
        public virtual Kullanici YoneticiKullanici { get; set; }

        [DataMember]
        public virtual ICollection<Kullanici> Kullanicilar { get; set; }
    }
}