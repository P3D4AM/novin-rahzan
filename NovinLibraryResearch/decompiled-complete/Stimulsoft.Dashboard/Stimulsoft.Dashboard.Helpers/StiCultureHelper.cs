using System.Globalization;
using System.Threading;
using Stimulsoft.Report;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiCultureHelper
{
	internal static CultureInfo Set(StiReport report)
	{
		if (string.IsNullOrWhiteSpace(report?.Culture))
		{
			return null;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo(report?.Culture, useUserOverride: false);
		}
		catch
		{
		}
		return currentCulture;
	}

	internal static void Restore(CultureInfo culture)
	{
		if (culture != null)
		{
			Thread.CurrentThread.CurrentCulture = culture;
		}
	}
}
