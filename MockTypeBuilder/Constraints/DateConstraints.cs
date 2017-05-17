using System;
using MockTypeBuilder.Constraints;

namespace MockTypeBuilder.Constraints
{
    public class DateConstraints
    {
        public DatePeriod DatePeriod { get; set; }
        public DateTime MinDateTime { get; set; }
        public DateTime MaxDateTime { get; set; }
    }
}
