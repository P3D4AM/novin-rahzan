using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiLargeHeightAtPageAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "On");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiLargeHeightAtPageActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiPage stiPage)
		{
			if (stiPage.LargeHeightFactor == 1)
			{
				stiPage.LargeHeightFactor = 2;
			}
			stiPage.LargeHeight = true;
		}
	}
}
