using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required(ErrorMessage = "الرجاء ادخال اسم المريض")]
        [Display(Name = "اسم المريض")]
        
        public string Name { get; set; }
                
        [Display(Name = "الرقم القومي")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "الرقم القومي يجب أن يحتوي علي 14 رقم")]
        public string SSNNew{ get; set; }
        //[Display(Name = "الرقم القومي")]
        //[Required(ErrorMessage = "الرجاء ادخال الرقم القومي")]
        //[RegularExpression(@"^\d{14}$", ErrorMessage = "الرقم القومي يجب أن يحتوي علي 14 رقم")]
        //public Int64 SSNNew { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال تاريخ الميلاد")]
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "الرجاء ادخال اسم رب الأسرة")]
        [Display(Name = "رب الأسرة")]
        public string Shepherd { get; set; }


        private bool? isAlife;
        [Display(Name="علي قيد الحياة؟ ")]
        public bool isalife
        { 
            get { return isAlife ?? true; }
            set { isAlife = value; } 
        }


        private DateTime? dateCreated;
        [Display(Name = "تاريخ إضافة المريض")]
        public DateTime DateCreated
        {
            get { return dateCreated ?? DateTime.Now;}
            set { dateCreated = value; }
        }
        public double PMTotalcost { get; set; }

        public virtual ICollection<Abanoub> Abanoubs { get; set; }
        public virtual ICollection<PatientDisease> PatientDiseases { get; set; }
        public virtual ICollection<PatientMedicineTest> PatientMedicineTests { get; set; }

        public virtual ICollection<PatientSurgery> PatientSurgeries { get; set; }

    }
}