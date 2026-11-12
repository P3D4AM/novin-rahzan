using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDeleteComponentAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Delete");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiDeleteComponentActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiComponent { Parent: not null } stiComponent)
		{
			stiComponent.Parent.Components.Remove(stiComponent);
			if (report != null && report.Designer != null)
			{
				report.Designer.Refresh();
			}
		}
	}
}
