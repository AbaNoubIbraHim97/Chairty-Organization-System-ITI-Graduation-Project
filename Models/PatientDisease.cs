using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PatientDisease
    {
        // [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "الرجاء أختر المريض")]
        [Display(Name = "اسم المريض:")]
        public int PatientID { get; set; }
         [Key, Column(Order = 2)]
        [Required(ErrorMessage = "الرجاء أختيار المرض ")]
        public int DiseasesID { get; set; }
        public virtual Diseases Diseases { get; set; }
        public virtual Patient Patient { get; set; }
    }
}