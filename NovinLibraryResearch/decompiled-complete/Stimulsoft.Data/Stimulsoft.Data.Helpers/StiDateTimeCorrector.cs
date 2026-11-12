using System;

namespace Stimulsoft.Data.Helpers;

public static class StiDateTimeCorrector
{
	public static DateTime Correct(DateTime dateTime)
	{
		return dateTime.Date;
	}
}
