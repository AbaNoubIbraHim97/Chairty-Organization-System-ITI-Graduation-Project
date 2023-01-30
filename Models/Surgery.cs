using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Surgery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال اسم العملية")]
        [Display(Name = "اسم العملية")]
        public string Name { get; set; }
        public virtual ICollection<PatientSurgery> PatientSurgeries { get; set; }


        //[Required(ErrorMessage = "الرجاء ادخال مكان العملية")]
        //[Display(Name = "المكان")]
        //public string Place { get; set; }


        //[Required(ErrorMessage = "الرجاء ادخال اسم الدكتور")]
        //[Display(Name = "اسم الدكتور")]
        //public string DoctorName { get; set; }


        //[Required(ErrorMessage = "الرجاء ادخال التكلفة")]
        //public double Cost { get; set; }




        //  public virtual ICollection<Patient> Patients { get; set; }

    }
}