using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PatientSurgery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        //[Key, Column(Order = 1)]
        [Required(ErrorMessage = "الرجاء أختر المريض")]
        [Display(Name ="اسم المريض:")] 
        public int PatientID { get; set; }
       // [Key, Column(Order = 2)]
        [Required(ErrorMessage = "الرجاء أختر العملية")]
        public int SurgeryID { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال مكان العملية")]
        [Display(Name = "المكان")]
        public string Place { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال اسم الدكتور")]
        [Display(Name = "اسم الدكتور")]
        public string DoctorName { get; set; }

        [Display(Name = "تكلفة العملية")]

        [Required(ErrorMessage = "الرجاء ادخال التكلفة")]
        public double Cost { get; set; }
        private DateTime? dateCreated;
        [Display(Name = "تاريخ العملية")]

        public DateTime DateCreated
        {
            get { return dateCreated ?? DateTime.Now; }
            set { dateCreated = value; }
        }
        public virtual Surgery Surgery { get; set; }
        public virtual Patient Patient { get; set; }
    }
}