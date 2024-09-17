using System;
namespace UmojaCampus.Shared.Helpers
{
    public static class StringHelper
    {
		public static string CalculateDuration(DateTime fromDate, DateTime toDate)
		{
			// Calculate the difference in years and months
			int years = toDate.Year - fromDate.Year;
			int months = toDate.Month - fromDate.Month;

			// Adjust the year and month difference if necessary
			if (months < 0)
			{
				years--;
				months += 12;
			}

			// Adjust months if the day of the toDate is less than fromDate
			if (toDate.Day < fromDate.Day)
			{
				months--;
				if (months < 0)
				{
					years--;
					months += 12;
				}
			}

			// Determine the largest unit of time to return
			if (years > 0)
			{
				return years == 1 ? "1 year" : $"{years} years";
			}
			else
			{
				return months == 1 ? "1 month" : $"{months} months";
			}
		}

	}
}
