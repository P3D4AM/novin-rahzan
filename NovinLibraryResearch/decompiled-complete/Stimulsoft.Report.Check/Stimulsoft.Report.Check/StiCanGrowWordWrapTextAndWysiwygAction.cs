using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCanGrowWordWrapTextAndWysiwygAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Change");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiCanGrowWordWrapTextAndWysiwygActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiText)
		{
			((StiText)element).TextQuality = StiTextQuality.Wysiwyg;
		}
	}
}
