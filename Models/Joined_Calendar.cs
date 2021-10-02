using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MHRS.Models
{
    public class Joined_Calendar
    {
        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public Nullable<DateTime> End { get; set; }

        public string ThemeColor  { get; set; }

        public bool IsFullDay { get; set; }
    }
}