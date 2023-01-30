using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Donations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Display(Name = "اسم المتبرع")]
        [Required(ErrorMessage = "الرجاء ادخال اسم المتبرع")]

        public string DonarName { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال اسم المستلم")]
        [Display(Name = "اسم المستلم")]
        public string RecieverName { get; set; }


        [Required(ErrorMessage = "الرجاء ادخال القيمة المدفوعة")]

        [Display(Name = "المبلغ المدفوع")]
        public double Value { get; set; }
        private DateTime? dateCreated;
        [Display(Name = "تاريخ إضافة التبرع")]
        public DateTime DateCreated
        {
            get { return dateCreated ?? DateTime.Now; }
            set { dateCreated = value; }
        }

    }
}