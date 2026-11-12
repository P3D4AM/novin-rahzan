using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDeleteDataRelationAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Delete");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiDeleteDataRelationActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiDataRelation relation)
		{
			report.Dictionary.Relations.Remove(relation);
		}
	}
}
