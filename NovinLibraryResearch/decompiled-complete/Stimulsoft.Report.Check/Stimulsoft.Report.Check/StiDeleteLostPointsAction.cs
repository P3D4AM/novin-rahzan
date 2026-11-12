using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDeleteLostPointsAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Delete");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiDeleteLostPointsActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiPage page))
		{
			return;
		}
		List<StiPointPrimitive> lostPointsOnPage = StiLostPointsOnPageCheck.GetLostPointsOnPage(page);
		foreach (StiPointPrimitive item in lostPointsOnPage)
		{
			item.Parent.Components.Remove(item);
		}
	}
}
