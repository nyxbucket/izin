using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace izin.Data
{
    [Table("KullaniciRol")]
    [DataContract(IsReference = true)]
    public class KullaniciRol
    {
        [DataMember]
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int RolId { get; set; }

        [DataMember]
        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; }

        [DataMember]
        [Required]
        public int KullaniciId { get; set; }

        [DataMember]
        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; }


    }
}