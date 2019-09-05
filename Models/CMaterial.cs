using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CMaterial
    {        
        public int materialId // Unique identity
        { get; set; }

        [Display(Name = "Material")]
        [Required(ErrorMessage = "Ange namn på material")]
        public string material // Materialnamn
        { get; set; }

        [Display(Name = "Kortnamn (ingår i artikelnummer)")]
        [Required(ErrorMessage = "Ange kortnamn")]
        public string materialShort // Kortnamn för material
        { get; set; }

    }
}