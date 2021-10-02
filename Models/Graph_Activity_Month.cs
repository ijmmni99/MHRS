using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MHRS.Models
{

    [DataContract]
    public class Graph_Activity_Month
    {
        public Graph_Activity_Month(string month, Nullable<decimal> total)
        {
            this.acMonth = month;
            this.Y = total;
        }
        [DataMember(Name = "label")]
        public string acMonth = "";

        [DataMember(Name = "y")]
        public Nullable<decimal> Y = null;

    }

        
}