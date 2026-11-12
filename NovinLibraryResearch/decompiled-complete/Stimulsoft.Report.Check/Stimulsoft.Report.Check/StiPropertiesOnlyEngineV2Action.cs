using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPropertiesOnlyEngineV2Action : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Off");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiPropertiesOnlyEngineV2ActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is IStiPageBreak stiPageBreak)
		{
			stiPageBreak.NewPageBefore = false;
			stiPageBreak.NewPageAfter = false;
			stiPageBreak.NewColumnAfter = false;
			stiPageBreak.NewColumnBefore = false;
			stiPageBreak.SkipFirst = true;
		}
	}
}
