using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiAllowHtmlTagsInTextAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "On");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiAllowHtmlTagsInTextActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiText)
		{
			((StiText)element).AllowHtmlTags = true;
		}
	}
}
