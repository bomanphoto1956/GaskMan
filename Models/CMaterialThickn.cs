using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CMaterialThickn
    {        
        public int materialThicknId // Primary key
        { get; set; }

        [Display(Name = "Material")]
        [Required(ErrorMessage = "Välj material")]
        public int materialSizeId // Foreign key to material size
        { get; set; }

        [Display(Name = "Beskrivning")]
        [Required(ErrorMessage = "Ange beskrivning")]
        public string description //Human readable description of material and size
        { get; set; }

        [Display(Name = "Kortnamn (ingår i artikelnummer)")]
        [Required(ErrorMessage = "Ange kortnamn")]
        public string thicknShort // Short to material size (in order to build article ID)
        { get; set; }

        [Display(Name = "Tjocklek (i mm)")]
        [Required(ErrorMessage = "Ange tjocklek")]
        public string thickness //Thickness of material in cm
        { get; set; }

        [Display(Name = "Inköpspris")]
        [Required(ErrorMessage = "Ange inköpspris")]
        public string buyPrice // Inprice
        { get; set; }

        [Display(Name = "Försäljningspris")]
        [Required(ErrorMessage = "Ange försäljningspris")]
        public string sellPrice // Sales price
        { get; set; }

        [Display(Name = "Skärtid (i m/minut)")]
        [Required(ErrorMessage = "Ange skärtid")]
        public string cuttingTime // In meter/sek
        { get; set; }
    }
}