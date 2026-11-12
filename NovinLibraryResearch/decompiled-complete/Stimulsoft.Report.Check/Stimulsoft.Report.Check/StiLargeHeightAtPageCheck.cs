using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiLargeHeightAtPageCheck : StiPageCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiLargeHeightAtPageCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiLargeHeightAtPageCheckLong"), base.Element);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		StiPage stiPage = base.Element as StiPage;
		if (!(stiPage is IStiForm))
		{
			double num = 0.0;
			int num2 = 0;
			foreach (StiComponent component in stiPage.Components)
			{
				if (component is StiBand && !((StiBand)component).IsCross)
				{
					num += component.Height;
					num2++;
				}
			}
			if (stiPage.Report.Info.ShowHeaders && num2 > 0)
			{
				num += stiPage.GridSize * (double)num2 * 2.0;
			}
			num += ((num2 > 1) ? (stiPage.GridSize * (double)num2 - 1.0) : stiPage.GridSize);
			double height = stiPage.Height;
			height *= (double)((!stiPage.LargeHeight) ? 1 : stiPage.LargeHeightFactor);
			if (height - num <= height * 0.1)
			{
				return true;
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (Check())
		{
			StiLargeHeightAtPageCheck stiLargeHeightAtPageCheck = new StiLargeHeightAtPageCheck();
			stiLargeHeightAtPageCheck.Element = obj;
			stiLargeHeightAtPageCheck.Actions.Add(new StiLargeHeightAtPageAction());
			return stiLargeHeightAtPageCheck;
		}
		return null;
	}
}
