using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Check;

public class StiBuyProductAction : StiAction
{
	public override string Name => StiLocalization.Get("Cloud", "ButtonPurchase");

	public override string Description => StiLocalization.Get("Cloud", "ButtonPurchase");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		string text = (StiLocalization.IsRussian ? "ru" : "en");
		StiProcess.Start("https://www.stimulsoft.com/" + text + "/online-store");
	}
}
