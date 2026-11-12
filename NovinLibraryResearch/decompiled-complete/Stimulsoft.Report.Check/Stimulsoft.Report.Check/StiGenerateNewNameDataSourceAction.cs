using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGenerateNewNameDataSourceAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "NewName");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiGenerateNewNameDataSourceActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiDataSource stiDataSource)
		{
			stiDataSource.Name = StiNameCreation.CreateName(report, StiNameCreation.GenerateName(stiDataSource), addOne: true, removeIncorrectSymbols: true, forceAdvancedNamingRule: true);
		}
	}
}
