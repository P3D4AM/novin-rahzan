using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDeletePageAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Delete");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiDeletePageActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiPage)
		{
			report.Pages.Remove(element as StiPage);
		}
	}
}
