using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiVerySmallSizesOfComponentsAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "StiVerySmallSizesOfComponentsShort");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiVerySmallSizesOfComponentsLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiComponent stiComponent && report?.Designer?.Info != null)
		{
			if (stiComponent.Width < report.Designer.Info.GridSize)
			{
				stiComponent.Width = report.Designer.Info.GridSize;
			}
			if (stiComponent.Height < report.Designer.Info.GridSize)
			{
				stiComponent.Height = report.Designer.Info.GridSize;
			}
			if (report.Designer.Info.AlignToGrid && stiComponent.Page != null)
			{
				stiComponent.Page.AlignToGrid(stiComponent);
			}
		}
	}
}
