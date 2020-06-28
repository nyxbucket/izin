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
    [Table("Durum")]
    [DataContract(IsReference = true)]
    public class Durum
    {
        [DataMember]
        [Editable(false)]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Aciklama { get; set; }

        
    }
}