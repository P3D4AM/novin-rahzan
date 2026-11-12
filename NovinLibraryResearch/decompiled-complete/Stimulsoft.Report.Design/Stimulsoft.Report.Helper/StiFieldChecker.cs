using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Helper;

public static class StiFieldChecker
{
	public static string AddressRequire => string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiHint.CreateHint(StiLocalization.Get("Report", "Address")));

	public static string NameRequire => string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiHint.CreateHint(StiLocalization.Get("Report", "LabelName")));

	public static string AliasRequire => string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Alias")));

	public static string NameExists(string name)
	{
		return string.Format(StiLocalization.Get("Errors", "NameExists"), name);
	}
}
