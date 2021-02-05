using System;

namespace CSharpNewAPI.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dateTime)
        {
            DateTime today=DateTime.Today;
            int age =(int) Math.Truncate((float) (today.Subtract(dateTime).Days) / 365);
            return age;
        }
    }
}