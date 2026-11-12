using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPrintOnPreviousPageAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Off");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiPrintOnPreviousPageLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiPage)
		{
			((StiPage)element).PrintOnPreviousPage = false;
		}
	}
}
