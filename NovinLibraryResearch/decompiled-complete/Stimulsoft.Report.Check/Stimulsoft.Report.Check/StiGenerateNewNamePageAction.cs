using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGenerateNewNamePageAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "NewName");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiGenerateNewNamePageActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiComponent stiComponent)
		{
			stiComponent.Name = StiNameCreation.CreateName(report, StiNameCreation.GenerateName(stiComponent), addOne: true, removeIncorrectSymbols: true, forceAdvancedNamingRule: true);
		}
	}
}
