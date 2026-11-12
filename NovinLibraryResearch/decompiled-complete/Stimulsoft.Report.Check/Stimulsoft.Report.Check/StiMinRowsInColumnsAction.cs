using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiMinRowsInColumnsAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Zero");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiMinRowsInColumnsActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiDataBand)
		{
			((StiDataBand)element).MinRowsInColumn = 0;
		}
	}
}
