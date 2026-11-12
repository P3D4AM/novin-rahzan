using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGoToCodeAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Code");

	public override string Description => StiLocalizationExt.Get("CheckActions", "GotoCodeLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
	}
}
