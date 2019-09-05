using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CCost
    {
        public int workingCostId // Primary key
        { get; set; }

        [Display(Name = "Timdebitering skärtid netto")]
        [Required(ErrorMessage = "Skriv in timdebitering skärtid netto")]
        public string cuttingHourNet // Net cost for cutting
        { get; set; }

        [Display(Name = "Timdebitering skärtid brutto")]
        [Required(ErrorMessage = "Skriv in timdebitering skärtid brutto")]
        public string cuttingHourSales // Sales price for cutting
        { get; set; }

        [Display(Name = "Timdebitering hantering netto")]
        [Required(ErrorMessage = "Skriv in timdebitering hantering netto")]
        public string handlingHourNet // Net cost for handling
        { get; set; }

        [Display(Name = "Timdebitering hantering brutto")]
        [Required(ErrorMessage = "Skriv in timdebitering hantering brutto")]
        public string handlingHourSales // Sales cost for handling
        { get; set; }

        [Display(Name = "Skärmarginal (mm)")]
        [Required(ErrorMessage = "Skriv in skärmarginal")]
        public string cuttingMargin // cutting margin (mm)
        { get; set; }

    }


}