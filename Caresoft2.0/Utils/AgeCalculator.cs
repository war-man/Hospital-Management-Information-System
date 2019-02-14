using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Utils
{
    public class AgeCalculator
    {
        public string CalculateYourAge(DateTime Dob)
        {
            if (Dob != null)
            {
                DateTime Now = DateTime.Now;
                int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
                DateTime PastYearDate = Dob.AddYears(Years);
                int Months = 0;
                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }
                int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                int Hours = Now.Subtract(PastYearDate).Hours;
                int Minutes = Now.Subtract(PastYearDate).Minutes;
                int Seconds = Now.Subtract(PastYearDate).Seconds;
                //return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",

                if (Years > 0)
                {
                    return String.Format("{0} Year(s) {1} Month(s)",
                        Years, Months);
                }
                else if (Months > 0)
                {
                    return String.Format("{0} Month(s) {1} Day(s)", Months, Days);
                }
                else
                {
                    return String.Format("{0} Day(s)", Days);
                }
            }
            return "N/A";
        }
    }
}