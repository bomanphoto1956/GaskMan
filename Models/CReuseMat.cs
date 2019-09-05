using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CReuseMat
    {
        public int reuseMatId // Primary key
        { get; set; }
        [Display(Name = "Min diameter (centrumhål) mm")]
        [Required(ErrorMessage = "Skriv in minsta diameter")]
        public string minDiam // Min diameter
        { get; set; }

        [Display(Name = "Återanvändbart material (%)")]
        [Required(ErrorMessage = "Skriv in återanvändbart material")]
        public string reusePercentage // Reusable percentage
        { get; set; }

    }
}