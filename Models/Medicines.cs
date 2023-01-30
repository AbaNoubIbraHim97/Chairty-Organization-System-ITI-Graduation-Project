using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Medicines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال اسم الدواء")]

        [Display(Name = "اسم الدواء")]
        public string Name { get; set; }
        [Display(Name = "سعر الوحدة")]

        [Required(ErrorMessage = "الرجاء ادخال سعر الوحدة")]
        public double PricePerUnit { get; set; }
        [Required(ErrorMessage = "الرجاء أختر نوع الدواء")]
        [ForeignKey("MedicinesCategory")]
        public int MedicineCategoryID { get; set; }
        public virtual MedicinesCategory MedicinesCategory { get; set; }
        public virtual ICollection<PatientMedicineTest> PatientMedicineTests { get; set; }


    }
}