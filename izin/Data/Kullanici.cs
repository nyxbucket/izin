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
    [Table("Kullanici")]
    [DataContract(IsReference = true)]
    public class Kullanici
    {
        [DataMember]
        [Editable(false)]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string KullaniciAdi { get; set; }

        [DataMember]
        [Required]
        public string Parola { get; set; }

        [DataMember]
        [Required]
        public string Eposta { get; set; }

        [DataMember]
        [Required]
        public string AdSoyad { get; set; }

        [DataMember]
        public int? DepartmanId { get; set; }

        [DataMember]
        [ForeignKey("DepartmanId")]
        public virtual Departman Departman { get; set; }

        [DataMember]
        [Required]
        public bool AktifMi { get; set; }

        [DataMember]
        public virtual ICollection<Rol> Roller { get; set; }

        [DataMember]
        public virtual ICollection<Departman> Departmanlar { get; set; }
    }
}