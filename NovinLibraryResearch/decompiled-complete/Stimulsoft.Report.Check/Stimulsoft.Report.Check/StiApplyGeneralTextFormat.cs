using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiApplyGeneralTextFormat : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "StiApplyGeneralTextFormatShort");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiApplyGeneralTextFormatLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiText stiText)
		{
			stiText.TextFormat = new StiGeneralFormatService();
		}
	}
}
