using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiComponentStyleIsNotFoundAtComponentAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Fix");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiComponentStyleIsNotFoundOnComponentActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiComponent))
		{
			return;
		}
		StiComponent stiComponent = element as StiComponent;
		if (stiComponent.ComponentStyle != string.Empty && !stiComponent.Report.Styles.Contains(stiComponent.ComponentStyle))
		{
			stiComponent.ComponentStyle = string.Empty;
		}
		if (stiComponent is IStiOddEvenStyles stiOddEvenStyles)
		{
			if (stiOddEvenStyles.EvenStyle != string.Empty && !stiComponent.Report.Styles.Contains(stiOddEvenStyles.EvenStyle))
			{
				stiOddEvenStyles.EvenStyle = string.Empty;
			}
			if (stiOddEvenStyles.OddStyle != string.Empty && !stiComponent.Report.Styles.Contains(stiOddEvenStyles.OddStyle))
			{
				stiOddEvenStyles.OddStyle = string.Empty;
			}
		}
	}
}
