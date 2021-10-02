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
    using System.ComponentModel.DataAnnotations;

    public partial class Activity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Activity()
        {
            this.Activity_Student = new HashSet<Activity_Student>();
            this.Activity_Staff = new HashSet<Activity_Staff>();
            this.Recommended_Activity = new HashSet<Recommended_Activity>();
        }

        public long Activity_ID { get; set; }
        public string Activity_Name { get; set; }
        public string Descriptions { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime AcDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime AcTime { get; set; }

        public System.DateTime Activity_Date { get; set; }
        public string Activity_Status { get; set; }
        public byte[] Activity_Image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity_Student> Activity_Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity_Staff> Activity_Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recommended_Activity> Recommended_Activity { get; set; }
    }
}
