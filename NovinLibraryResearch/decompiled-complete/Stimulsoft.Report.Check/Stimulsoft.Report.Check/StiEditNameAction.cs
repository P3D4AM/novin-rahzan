using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiEditNameAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Edit");

	public override string Description => StiLocalizationExt.Get("CheckActions", "Edit");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (report.Designer != null)
		{
			StiReportCheckerErrorParameters parameters = new StiReportCheckerErrorParameters(elementName, "Name", element);
			report.Designer.ShowReportCheckerError(parameters);
		}
	}
}
