using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

internal static class StiLocHelper
{
	public static string LocAll => "(" + Loc.Get("Report", "RangeAll").Replace("&", "") + ")";
}
