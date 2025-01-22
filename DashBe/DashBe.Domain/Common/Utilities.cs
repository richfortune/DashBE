using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Common
{
    public static class Utilities
    {
        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@");
        }

        public static DateTime ConvertToUtc(DateTime dateTime, string timeZoneId)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTimeToUtc(dateTime, timezone);
        }

        public static int TrovaMassimo(int[] iNumeri) 
        {
            if(iNumeri != null || iNumeri.Length == 0)
            {
                throw new ArgumentException("L'array non può essere vuoto");
            }

            int iNumMax = iNumeri[0];

            foreach (var item in iNumeri)
            {
                if(item > iNumMax)
                {
                    iNumMax = item;
                }
            }

            return iNumMax;
        }
    }
}
