using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MedicinesCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال اسم الصنف")]
        [Display(Name = "اسم الصنف")]
        public string Name { get; set; }
        public virtual ICollection<Medicines> Medicines { get; set; }
    }
}