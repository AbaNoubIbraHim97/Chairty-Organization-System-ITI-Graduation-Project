using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TemporaryCashing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال  عدد الوحدات ")]
        [Display(Name = "عدد الوحدات")]
        public int NumberofUnites { get; set; }


        private DateTime? dateCreated;
        public DateTime DateCreated
        {
            get { return dateCreated ?? DateTime.Now; }
            set { dateCreated = value; }
        }


        [Required(ErrorMessage = "الرجاء ادخال  قيمة التكلفة ")]
        [Display(Name = "التكلفة")]
        public double Cost { get; set; }



        [Required(ErrorMessage = "الرجاء اختر  نوع الدواء ")]
        [Display(Name = "اسم الدواء")]
        [ForeignKey("Medicines")]
        public int MedicineID { get; set; }
        public virtual Medicines Medicines { get; set; }


        [Required(ErrorMessage = "الرجاء اختيار اسم المريض ")]
        [Display(Name = "اسم المريض")]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

    }
}