using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CGasket
    {
        public int gasketId // Primary key
        { get; set; }

        [Display(Name = "Packningstyp")]
        [Required(ErrorMessage = "Välj packningstyp")]
        public int gasketTypeId // Foreign key to gasketType
        { get; set; }

        [Display(Name = "Material")]
        [Required(ErrorMessage = "Välj material")]
        public int materialThicknId // Foreign key to material thickness -> materialSize -> material
        { get; set; }

        [Display(Name = "Ytterdiameter (i mm)")]
        [Required(ErrorMessage = "Skriv in ytterdiameter")]
        public String outerDiam // Outer diameter in mm
        { get; set; }

        [Display(Name = "Innerdiameter (i mm)")]
        [Required(ErrorMessage = "Skriv in innerdiameter")]
        public String innerDiam // Inner diameter in mm
        { get; set; }

        [Display(Name = "Antal yttre hål runt centrumhålet")]
        public int Type2SecHoleCount
        { get; set; }

        [Display(Name = "Diameter yttre hål")]
        public string Type2SecHoleDiam
        { get; set; }

        [Display(Name = "Längd (i mm)")]
        public string Type3GasketLength // Length of rectangular gaskets
        { get; set; }

        [Display(Name = "Bredd (i mm)")]
        public string Type3GasketWidth // Width of rectangular gaskets
        { get; set; }


        [Display(Name = "Återanvändbart material (i %)")]
        [Required(ErrorMessage = "Skriv in återanvändbart material")]
        public String reusableMaterial // Reusable material in percent
        { get; set; }

        [Display(Name = "Klippmarginal (i mm)")]
        [Required(ErrorMessage = "Skriv in klippmaterial")]
        public String cuttingMargin // Cutting margin in mm
        { get; set; }

        [Display(Name = "Standardprissättning")]
        public bool standardPriceProduct // Whether this is a standard price or not
        { get; set; }

        [Display(Name = "Hanteringstid (i sekunder)")]
        [Required(ErrorMessage = "Skriv in hanteringstid")]
        public String handlingTime // Handling time in seconds
        { get; set; }

        [Display(Name = "Försäljningspris")]
        [Required(ErrorMessage = "Skriv in försäljningspris")]
        public String price // Selling price
        { get; set; }

        [Display(Name = "Notering")]        
        public string note
        { get; set; }

        [Display(Name = "Beskrivning")]
        [Required(ErrorMessage = "Ange beskrivning")]
        public string description // beskrivning
        { get; set; }

        public Double materialCostMm2 //Material cost
        { get; set; }

        public Double materialMarginPercent
        { get; set; }

        public Double cuttingLengthOuterMm //Cutting length
        { get; set; }

        public Double cuttingLengthInnerMm //Cutting length
        { get; set; }

        public Double cuttingSpeedMSek //Cutting speed in m/sek
        { get; set; }

        public Double materialArea
        { get; set; }

    }


}