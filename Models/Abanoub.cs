using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Abanoub
    {
        public int ID { get; set; }
        public string PatientName { get; set; }
        //[ForeignKey("Patient")]
        //public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }
}