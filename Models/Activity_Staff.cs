//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MHRS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Activity_Staff
    {
        public long Activity_Staff_ID { get; set; }
        public string Staff_ID { get; set; }
        public long Activity_ID { get; set; }
        public string Staff_Name { get; set; }

        public string DisplayName
        {
            get
            {
                return this.Staff.Fname + " " + this.Staff.Lname;
            }
        }

        public virtual Activity Activity { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
