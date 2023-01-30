using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Diseases
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال نوع المرض")]
        [Display(Name = "نوع المرض")]
        public string Name { get; set; }
        public virtual ICollection<PatientDisease> PatientDiseases { get; set; }

        // public virtual ICollection<Patient> Patients { get; set; }

    }
}