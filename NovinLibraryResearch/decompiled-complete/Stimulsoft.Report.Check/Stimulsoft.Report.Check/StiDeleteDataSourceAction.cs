using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDeleteDataSourceAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Delete");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiDeleteDataSourceActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiDataSource dataSource)
		{
			report.Dictionary.DataSources.Remove(dataSource);
		}
	}
}
