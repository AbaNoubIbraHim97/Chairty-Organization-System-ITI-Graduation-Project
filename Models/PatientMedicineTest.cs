using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PatientMedicineTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "الرجاء اختيار اسم المريض")]
        [Display(Name = "اسم المريض")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        // [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "الرجاء اختيار اسم الدواء")]
        [Display(Name = "اسم الدواء")]
        public int MedicineID { get; set; }
        public Medicines Medicines { get; set; }

        // public virtual Patient Patient { get; set; }
        // public virtual Medicines Medicines { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال عدد الوحدات")]
        [Display(Name = "عدد الوحدات")]
        public int NumberOfUnits { get; set; }

        [Display(Name = "التكلفة الكلية")]

        public double CostPerMedicineType { get; set; }
       
        

    }
}