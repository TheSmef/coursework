using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR.Models.Attributes
{
    public class DateAttribute : RangeAttribute
    {
        public DateAttribute(int start, int end)
          : base(typeof(DateTime), DateTime.Now.AddYears(-start).ToShortDateString(), DateTime.Now.AddYears(end).ToShortDateString()) { }
    }
}
