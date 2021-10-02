using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MHRS.Models
{
    [DataContract]
    public class Graph_Activity
    {
        public Graph_Activity(string activityName, Nullable<decimal> total)
        {
            this.acName = activityName;
            this.Y = total;
        }
        [DataMember(Name = "label")]
        public string acName = "";

        [DataMember(Name = "y")]
        public Nullable<decimal> Y = null;

    }
}