using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GaskMan.Models
{
    public class CUser
    {
        [Display(Name = "AnvändarId")]
        [Required(ErrorMessage = "Välj användare")]
        public string AnvID
        {
            get; set;
        }

        [Display(Name = "Användare")]
        public string Reparator
        {
            get; set;
        }

        public string RepKatID
        {
            get; set;
        }
        [Display(Name = "Roll")]
        [Required(ErrorMessage = "Välj roll")]
        public int gasketLevel // Access level for gasket calculation. 0 (or null) = no access, 5 = user, 10 = administrator
        {
            get; set;
        }
    }
}



