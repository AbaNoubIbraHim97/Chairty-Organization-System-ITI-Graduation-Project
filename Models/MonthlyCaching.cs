using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MonthlyCaching
    {
        
        //public int ID { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public virtual Patient Patient { get; set; }
       [Key, Column(Order = 1)]
        private DateTime? dateCreated;
        public DateTime DateCreated
        {
            get { return dateCreated ?? DateTime.Now; }
            set { dateCreated = value; }
        }


        [Required(ErrorMessage = "الرجاء ادخال قيمة التكلفة")]
        [Display(Name = "التكلفة")]
        public double Cost { get; set; }
        public bool status { get; set; }

    }
}