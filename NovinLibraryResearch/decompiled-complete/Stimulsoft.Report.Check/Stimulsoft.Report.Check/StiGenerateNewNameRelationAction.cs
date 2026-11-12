using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGenerateNewNameRelationAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "NewName");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiGenerateNewNameRelationActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiDataRelation stiDataRelation)
		{
			stiDataRelation.Name = StiNameCreation.CreateName(report, StiNameCreation.GenerateName(stiDataRelation), addOne: true, removeIncorrectSymbols: true, forceAdvancedNamingRule: true);
		}
	}
}
