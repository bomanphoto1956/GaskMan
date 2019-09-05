using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CMaterialSize
    {
        public int materialSizeId // Primary key
        { get; set; }

        [Display(Name = "Material")]
        [Required(ErrorMessage = "Välj material")]
        public int materialId // Foreign key to material
        { get; set; }

        [Display(Name = "Beskrivning")]
        [Required(ErrorMessage = "Ange beskrivning")]
        public string description //Human readable description of material and size
        { get; set; }

        [Display(Name = "Kortnamn (ingår i artikelnummer)")]
        [Required(ErrorMessage = "Ange kortnamn")]
        public string sizeShort // Short to material size (in order to build article ID)
        { get; set; }

        [Display(Name = "Materiallängd (i meter)")]
        [Required(ErrorMessage = "Ange längd på material")]
        public String materialLength //Length of material (in meters)
        { get; set; }

        [Display(Name = "Materialbredd (i meter)")]
        [Required(ErrorMessage = "Ange bredd på material")]
        public String materialWidth //Length of material (in meters)
        { get; set; }

        [Display(Name = "Förval (visas överst i vallista)")]
        public bool defaultVal //If this is a default value (shall be in top of dropdown)
        { get; set; }



        public bool fromStrToDec(string sValue, ref Decimal dValue)
        {
            return fromStrToDec(sValue, ref dValue, 0.01M , 10);
        }

        public bool fromStrToDec(string sValue, ref Decimal dValue, Decimal minValue, Decimal maxValue)
        {
            dValue = 0;
            sValue = sValue.Replace(".", ",");
            if (Decimal.TryParse(sValue, out dValue))
            {
                if (dValue >= minValue && dValue <= maxValue)
                    return true;
            }
            return false;

        }
    }



}