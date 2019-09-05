using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GaskMan.Models
{
    public class CLogin
    {
        [DataMember]
        public string AnvID // Unique identity
        { get; set; }

        [DataMember]
        public string pwd // Password
        { get; set; }

        [DataMember]
        public string reparator // Name of the reparator
        { get; set; }

        [DataMember]
        public string ident // Ident that is used in later calls
        { get; set; }       // To be stored in the client

        [DataMember]
        public int loginLevel // Get the login level from API
        { get; set; }
    }
}


