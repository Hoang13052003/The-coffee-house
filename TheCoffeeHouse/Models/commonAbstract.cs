using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models
{
    public class commonAbstract
    {
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Updates_By { get; set; }
        public Nullable<System.DateTime> Updates_Date { get; set; }
    }
}